using DependencyStore.Core.Repositories;
using DependencyStore.Core.Repositories.Contracts;
using DependencyStore.Core.Services;
using DependencyStore.Core.Services.Contracts;
using DependencyStore.Web.Models;

namespace DependencyStore.Core.Orders.Create;

public class Handler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;

    public Handler(
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IPromoCodeRepository promoCodeRepository,
        IDeliveryFeeService deliveryFeeService)
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