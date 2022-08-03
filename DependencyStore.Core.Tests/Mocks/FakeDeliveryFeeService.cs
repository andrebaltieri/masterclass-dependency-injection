using DependencyStore.Core.Services.Contracts;

namespace DependencyStore.Core.Tests.Mocks;

public class FakeDeliveryFeeService : IDeliveryFeeService
{
    public async Task<decimal> GetDeliveryFeeAsync(string zipCode)
    {
        await Task.Delay(0);
        return 5;
    }
}