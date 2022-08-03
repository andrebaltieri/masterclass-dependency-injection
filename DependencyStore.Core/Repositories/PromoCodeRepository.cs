using Dapper;
using DependencyStore.Core.Repositories.Contracts;
using DependencyStore.Web.Models;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Core.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly SqlConnection _connection;

    public PromoCodeRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<PromoCode?> GetAsync(string promoCode)
    {
        const string query = "SELECT * FROM PROMO_CODES WHERE CODE=@code";
        return await _connection.QueryFirstAsync<PromoCode>(query, new { code = promoCode });
    }
}