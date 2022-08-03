using Dapper;
using DependencyStore.Web.Models;
using Microsoft.Data.SqlClient;
using RestSharp;

namespace DependencyStore.Core.Orders.Create;

public class Handler
{
    public async Task<Response> HandleAsync(Request request)
    {
        // #1 - Recupera o cliente
        Customer customer = null;
        await using (var conn = new SqlConnection("CONN_STRING"))
        {
            const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
            customer = await conn.QueryFirstAsync<Customer>(query, new { id = request.Customer });
        }

        // #2 - Calcula o frete
        decimal deliveryFee = 0;
        var client = new RestClient("https://consultafrete.io/cep/");
        var req = new RestRequest()
            .AddJsonBody(new
            {
                request.ZipCode
            });
        deliveryFee = await client.PostAsync<decimal>(req, new CancellationToken());
        // Nunca é menos que R$ 5,00
        if (deliveryFee < 5)
            deliveryFee = 5;

        // #3 - Calcula o total dos produtos
        decimal subTotal = 0;
        const string getProductQuery = "SELECT [Id], [Name], [Price] FROM PRODUCT WHERE ID=@id";
        for (var p = 0; p < request.Products.Length; p++)
        {
            Product product;
            await using (var conn = new SqlConnection("CONN_STRING"))
                product = await conn.QueryFirstAsync<Product>(getProductQuery, new { Id = p });

            subTotal += product.Price;
        }

        // #4 - Aplica o cupom de desconto
        decimal discount = 0;
        await using (var conn = new SqlConnection("CONN_STRING"))
        {
            const string query = "SELECT * FROM PROMO_CODES WHERE CODE=@code";
            var promo = await conn.QueryFirstAsync<PromoCode>(query, new { code = request.PromoCode });
            if (promo.ExpireDate > DateTime.Now)
                discount = promo.Value;
        }

        // #5 - Gera o pedido
        var order = new Order();
        order.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
        order.Date = DateTime.Now;
        order.DeliveryFee = deliveryFee;
        order.Discount = discount;
        order.Products = request.Products;
        order.SubTotal = subTotal;

        // #6 - Calcula o total
        order.Total = subTotal - discount + deliveryFee;

        // #7 - Retorna
        return new Response($"Pedido {order.Code} gerado com sucesso!");
    }
}