using DependencyStore.Web.Models;

namespace DependencyStore.Core.Repositories.Contracts;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetByIdAsync(int[] ids);
}