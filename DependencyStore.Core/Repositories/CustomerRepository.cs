using Dapper;
using DependencyStore.Core.Repositories.Contracts;
using DependencyStore.Web.Models;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Core.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly SqlConnection _connection;

    public CustomerRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
        return await _connection
            .QueryFirstAsync<Customer>(query, new
            {
                id = email
            });
    }
}