using DependencyStore.Core.Orders.Create;

namespace DependencyStore.Core.Tests.Orders;

[TestClass]
public class CreateOrderRequestTests
{
    [TestMethod]
    public void ShouldFailOnInvalidRequest()
    {
        var request = new Request
        {
            Customer = "invalid",
            Products = Array.Empty<int>(),
            PromoCode = string.Empty,
            ZipCode = "123"
        };
        request.Validate();
        
        Assert.AreEqual(false, request.IsValid);
    }
    
    [TestMethod]
    public void ShouldSuccessOnValidRequest()
    {
        var request = new Request
        {
            Customer = "email@email.com",
            Products = Array.Empty<int>(),
            PromoCode = string.Empty,
            ZipCode = "13456-888"
        };
        request.Validate();
        
        Assert.AreEqual(true, request.IsValid);
    }
}