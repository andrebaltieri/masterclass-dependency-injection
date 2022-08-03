using Flunt.Extensions.Br.Validations;
using Flunt.Notifications;
using Flunt.Validations;

namespace DependencyStore.Core.Orders.Create;

public class Request : Notifiable<Notification>
{
    public string Customer { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string PromoCode { get; set; } = string.Empty;
    public int[] Products { get; set; } = Array.Empty<int>();

    public void Validate()
    {
        AddNotifications(new Contract<Notification>()
            .IsEmail(Customer, "Email", "E-mail inválido")
            .IsZipCode(ZipCode, "ZipCode", "CEP inválido"));
    }
}