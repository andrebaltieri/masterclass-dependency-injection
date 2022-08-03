using Dapper;
using DependencyStore.Web.Models;
using Microsoft.Data.SqlClient;
using RestSharp;

namespace DependencyStore.Core.Orders.Create;

public class CustomerRepository
{
    private readonly SqlConnection _connection;

    public CustomerRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
        return await _connection
            .QueryFirstAsync<Customer>(query, new
            {
                id = email
            });
    }
}

public class Configuration
{
    public string DeliveryFeeServiceUrl { get; set; } = string.Empty;
}

public class DeliveryFeeService
{
    private readonly Configuration _configuration;

    public DeliveryFeeService(Configuration configuration)
        => _configuration = configuration;

    public async Task<decimal> GetDeliveryFeeAsync(string zipCode)
    {
        decimal deliveryFee = 0;
        var client = new RestClient(_configuration.DeliveryFeeServiceUrl);
        var req = new RestRequest()
            .AddJsonBody(new
            {
                zipCode
            });
        deliveryFee = await client.PostAsync<decimal>(req);
        
        // Nunca é menos que R$ 5,00
        if (deliveryFee < 5)
            deliveryFee = 5;

        return deliveryFee;
    }
}

public class ProductRepository
{
    private readonly SqlConnection _connection;

    public ProductRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<IEnumerable<Product>> GetByIdAsync(int[] ids)
    {
        const string getProductQuery = "SELECT [Id], [Name], [Price] FROM PRODUCT WHERE ID IN(@ids)";
        return await _connection.QueryAsync<Product>(getProductQuery, new { Ids = ids });
    }
}

public class PromoCodeRepository
{
    private readonly SqlConnection _connection;

    public PromoCodeRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<PromoCode?> GetAsync(string promoCode)
    {
        const string query = "SELECT * FROM PROMO_CODES WHERE CODE=@code";
        return await _connection.QueryFirstAsync<PromoCode>(query, new { code = promoCode });
    }
}

public class Handler
{
    private readonly CustomerRepository _customerRepository;
    private readonly ProductRepository _productRepository;
    private readonly PromoCodeRepository _promoCodeRepository;
    private readonly DeliveryFeeService _deliveryFeeService;

    public Handler(
        CustomerRepository customerRepository,
        ProductRepository productRepository,
        PromoCodeRepository promoCodeRepository,
        DeliveryFeeService deliveryFeeService)
    {
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _promoCodeRepository = promoCodeRepository;
        _deliveryFeeService = deliveryFeeService;
    }

    public async Task<Response> HandleAsync(Request request)
    {
        // #1 - Recupera o cliente
        var customer = await _customerRepository.GetByEmailAsync(request.Customer);

        // #2 - Calcula o frete
        var deliveryFee = await _deliveryFeeService.GetDeliveryFeeAsync(request.ZipCode);
        
        // #3 - Calcula o total dos produtos
        var products = await _productRepository.GetByIdAsync(request.Products);
        var subTotal = products.Sum(product => product.Price);

        // #4 - Aplica o cupom de desconto
        decimal discount = 0;
        var promo = await _promoCodeRepository.GetAsync(request.PromoCode);
        if (promo != null && promo.ExpireDate > DateTime.Now)
            discount = promo.Value;

        // #5 - Gera o pedido
        var order = new Order
        {
            Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 8),
            Date = DateTime.Now,
            DeliveryFee = deliveryFee,
            Discount = discount,
            Products = request.Products,
            SubTotal = subTotal,
            // #6 - Calcula o total
            Total = subTotal - discount + deliveryFee
        };

        // #7 - Retorna
        return new Response($"Pedido {order.Code} gerado com sucesso!");
    }
}