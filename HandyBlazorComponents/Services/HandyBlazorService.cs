

using Microsoft.JSInterop;

namespace HandyBlazorComponents.Services;

public class HandyBlazorService
{
    private readonly IJSRuntime jsRuntime;
    public HandyBlazorService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Reset forms that were dirty
    /// </summary>
    public async Task ResetFormStates()
    {
        await jsRuntime.InvokeVoidAsync("resetBeforeUnloads");
    }
}