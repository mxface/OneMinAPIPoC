using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ServiceUsageHistoryService : IServiceUsageHistoryService
{
    private readonly ICommonService _commonService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string _apiBaseUrl;
    private readonly string _accessKey;

    public ServiceUsageHistoryService(
        ICommonService commonService,
        HttpClient httpClient,
        IConfiguration config
    )
    {
        _commonService = commonService;
        _httpClient = httpClient;
        _config = config;

        _apiBaseUrl = _config["MxFace:ApiBaseUrl"] ?? string.Empty;
        _accessKey = _config["MxFace:AccessKey"] ?? string.Empty;
    }

    public async Task<ClientPackageDto> GetClientUsage()
    {
        var clientId = _commonService.GetClientId(); // This returns 4437

        Console.WriteLine($"AccessKey: {_accessKey}");
        Console.WriteLine($"ApiBaseUrl: {_apiBaseUrl}");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _accessKey
        );
        var response = await _httpClient.GetAsync(
            $"{_apiBaseUrl}/api/SubscriptionPlans/GetSubscriptionPackage?activeId=1&clientId={clientId}"
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting usage data: {response.StatusCode}");
        }

        var packages = await response.Content.ReadFromJsonAsync<List<ClientPackageDto>>();
        return packages?.FirstOrDefault() ?? new ClientPackageDto();
    }
}

