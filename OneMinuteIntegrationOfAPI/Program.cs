using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OneMinuteIntegrationOfAPI;
using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Services;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["MxFace:ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress),
});

// Allow services to access configuration values
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IServiceUsageHistoryService, ServiceUsageHistoryService>();

await builder.Build().RunAsync();
