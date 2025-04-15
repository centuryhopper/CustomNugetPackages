namespace HandyBlazorComponents.Models;

// TODO: copy over to fwc HandyBlazorComponents
public class ServiceResponses
{
    public record class GridValidationResponse(
        bool Flag,
        Dictionary<string, List<string>>? ErrorMessagesDict
    );

    public record class GeneralResponse(bool Flag, string Message);

    public record class GeneralResponseWithPayload(bool Flag, string Message, string Payload);

    public record class LoginResponse(bool Flag, string Token, string Message);
}

