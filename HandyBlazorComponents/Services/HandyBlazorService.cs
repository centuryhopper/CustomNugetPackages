using System.Security.Claims;
using System.Text.Json;
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

    public async Task StoreJwtExpiration(string jwtName, string jwtExpDateName, DateTime jwtExpDate)
    {
        if (jwtExpDate < DateTime.Now)
        {
            throw new Exception("Must pass in a jwtExpDate that is a DateTime in the future");
        }

        var jwtExp = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", jwtExpDateName);
        jwtExp ??= await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", jwtExpDateName);
        // if there's already a timestamp for the jwt then terminate right now
        if (!string.IsNullOrWhiteSpace(jwtExp))
        {
            return;
        }

        string? jwt = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", jwtName);
        jwt ??= await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", jwtName);

        // make sure the jwt is already stored in either the localstorage or sessionstorage
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return;
        }

        var jwtExpDateInMilliseconds = new DateTimeOffset(jwtExpDate).ToUnixTimeMilliseconds();
        await jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            jwtExpDateName,
            jwtExpDateInMilliseconds
        );
        await jsRuntime.InvokeVoidAsync(
            "sessionStorage.setItem",
            jwtExpDateName,
            jwtExpDateInMilliseconds
        );
    }
}
