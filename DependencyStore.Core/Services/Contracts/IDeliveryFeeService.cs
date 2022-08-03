namespace DependencyStore.Core.Services.Contracts;

public interface IDeliveryFeeService
{
    Task<decimal> GetDeliveryFeeAsync(string zipCode);
}