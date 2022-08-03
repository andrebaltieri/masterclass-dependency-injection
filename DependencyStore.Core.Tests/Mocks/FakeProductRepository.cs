using DependencyStore.Core.Repositories.Contracts;
using DependencyStore.Web.Models;

namespace DependencyStore.Core.Tests.Mocks;

public class FakeProductRepository : IProductRepository
{
    public async Task<IEnumerable<Product>> GetByIdAsync(int[] ids)
    {
        await Task.Delay(0);
        return new List<Product>
        {
            new()
            {
                Id = "PRD01",
                Name = "Produto 1",
                Price = 10
            },
            new()
            {
                Id = "PRD02",
                Name = "Produto 2",
                Price = 10
            },
            new()
            {
                Id = "PRD03",
                Name = "Produto 4",
                Price = 10
            },
        };
    }
}