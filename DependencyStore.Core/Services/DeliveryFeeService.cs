using DependencyStore.Core.Services.Contracts;
using RestSharp;

namespace DependencyStore.Core.Services;

public class DeliveryFeeService : IDeliveryFeeService
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