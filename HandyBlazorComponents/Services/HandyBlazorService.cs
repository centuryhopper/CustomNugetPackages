using System.Security.Claims;
using System.Text.Json;
using HandyBlazorComponents.Models;
using Microsoft.JSInterop;

namespace HandyBlazorComponents.Services;

public class HandyBlazorService
{
    private readonly IJSRuntime jsRuntime;

    public HandyBlazorService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task DownloadFile(string base64, HandyDownloadType handyDownloadType, string fileName)
    {
        string downloadType = handyDownloadType switch
        {
            HandyDownloadType.PDF => "application/pdf",
            HandyDownloadType.JPG => "image/jpg",
            HandyDownloadType.PNG => "image/png",
            _ => string.Empty,
        };

        fileName = handyDownloadType switch
        {
            HandyDownloadType.PDF => fileName + ".pdf",
            HandyDownloadType.JPG => fileName + ".jpg",
            HandyDownloadType.PNG => fileName + ".png",
            _ => string.Empty,
        };
        await jsRuntime.InvokeVoidAsync("FILE.downloadFile", base64, downloadType, fileName);
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
