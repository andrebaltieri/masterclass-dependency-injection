using DependencyStore.Core.Orders.Create;
using DependencyStore.Core.Tests.Mocks;

namespace DependencyStore.Core.Tests.Orders;

[TestClass]
public class CreateOrderHandlerTests
{
    [TestMethod]
    public async Task ShouldSuccessOnValidRequest()
    {
        var request = new Request
        {
            Customer = "email@email.com",
            Products = Array.Empty<int>(),
            PromoCode = string.Empty,
            ZipCode = "13456-888"
        };

        var handler = new Handler(
            new FakeCustomerRepository(),
            new FakeProductRepository(),
            new FakePromoCodeRepository(),
            new FakeDeliveryFeeService());

        var response = await handler.HandleAsync(request);
        Assert.IsNotNull(response);
    }
}