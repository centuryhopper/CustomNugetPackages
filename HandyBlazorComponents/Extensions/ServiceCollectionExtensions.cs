

using HandyBlazorComponents.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HandyBlazorComponents.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandyBlazorServices(this IServiceCollection services)
    {
        // Register your services here
        services.AddSingleton<HandyBlazorService>();

        return services;
    }
}
