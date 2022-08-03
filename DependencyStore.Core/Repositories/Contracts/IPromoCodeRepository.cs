using DependencyStore.Web.Models;

namespace DependencyStore.Core.Repositories.Contracts;

public interface IPromoCodeRepository
{
    Task<PromoCode?> GetAsync(string promoCode);
}