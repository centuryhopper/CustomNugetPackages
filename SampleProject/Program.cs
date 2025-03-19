global using CsvHelper.Configuration;
using Client.Models;
using Client.Services;
using HandyBlazorComponents.Abstracts;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleProject;

/*
dotnet tool install --global dotnet-outdated-tool
dotnet outdated

the commands above will tell you which of your nuget packages need to be updated
*/

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// builder.Services.AddSingleton<HandyGridStateAbstract<HandyGridEntity, TestClass>, GridStateService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
