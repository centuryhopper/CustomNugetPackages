
namespace HandyBlazorComponents.Models;

public class ServiceResponses
{
    public record class GridValidationResponse(bool Flag, Dictionary<string, List<string>>? ErrorMessagesDict);
    public record class GeneralResponse(bool Flag, string Message);
}