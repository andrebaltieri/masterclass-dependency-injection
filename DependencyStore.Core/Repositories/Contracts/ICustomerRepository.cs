using DependencyStore.Web.Models;

namespace DependencyStore.Core.Repositories.Contracts;

public interface ICustomerRepository
{
    Task<Customer?> GetByEmailAsync(string email);
}