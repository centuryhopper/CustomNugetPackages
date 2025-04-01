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

    public async Task StoreJwtExpiration(string jwtName, string jwtExpDateName)
    {
        string? jwt = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", jwtName);
        jwt ??= await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", jwtName);
        if (!string.IsNullOrWhiteSpace(jwt))
        {
            var claims = ParseClaimsFromJwt(jwt);
            var expClaimValue = int.Parse(claims.First(c => c.Type == "exp").Value);
            // convert to milliseconds
            expClaimValue *= 1000;
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", jwtExpDateName, expClaimValue);
            await jsRuntime.InvokeVoidAsync(
                "sessionStorage.setItem",
                jwtExpDateName,
                expClaimValue
            );
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }
}
