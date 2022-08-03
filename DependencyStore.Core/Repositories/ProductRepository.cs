using Dapper;
using DependencyStore.Core.Repositories.Contracts;
using DependencyStore.Web.Models;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Core.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SqlConnection _connection;

    public ProductRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<IEnumerable<Product>> GetByIdAsync(int[] ids)
    {
        const string getProductQuery = "SELECT [Id], [Name], [Price] FROM PRODUCT WHERE ID IN(@ids)";
        return await _connection.QueryAsync<Product>(getProductQuery, new { Ids = ids });
    }
}