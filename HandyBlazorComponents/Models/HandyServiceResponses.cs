namespace HandyBlazorComponents.Models;

// TODO: copy over to fwc HandyBlazorComponents
public class HandyServiceResponses
{
    public record class HandyGridValidationResponse(
        bool Flag,
        Dictionary<string, List<string>>? ErrorMessagesDict
    );

    public record class HandyGeneralResponse(bool Flag, string Message);

    public record class HandyGeneralResponseWithPayload(bool Flag, string Message, string Payload);

    public record class HandyLoginResponse(bool Flag, string Token, string Message);
}

