
namespace HandyBlazorComponents.Models;

public class ServiceResponses
{
    public record class GridValidationResponse(bool Flag, Dictionary<int, List<string>>? ErrorMessagesDict);
    public record class GeneralResponse(bool Flag, string Message);
}