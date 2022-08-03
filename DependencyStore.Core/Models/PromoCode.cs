namespace DependencyStore.Web.Models;

public class PromoCode
{
    public DateTime ExpireDate { get; set; } = DateTime.Now;
    public decimal Value { get; set; } = 0;
    public string Code { get; set; } = string.Empty;
}