namespace DependencyStore.Core.Orders.Create;

public class Response
{
    public Response()
    {
    }

    public Response(string message) => Message = message;

    public string Message { get; } = string.Empty;
}