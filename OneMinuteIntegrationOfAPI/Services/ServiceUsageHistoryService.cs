using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ServiceUsageHistoryService : IServiceUsageHistoryService
{
    private readonly ICommonService _commonService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private const string API_BASE_URL = "https://api.mxface.ai";
    private const string ACCESS_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ijg5NyIsInJvbGUiOiJBZG1pbiIsImNsYWltUm9sZSI6IkFkbWluIiwiY2xhaW1DbGllbnRJZCI6Ijg5NyIsIm5iZiI6MTc1NzA3NzU1NiwiZXhwIjoxODIwMTQ5NTU2LCJpYXQiOjE3NTcwNzc1NTZ9.A7C_BOgz2OrlnB2kRmg7HW7iKJfh_YJnioTscz2y7G0";

    public ServiceUsageHistoryService(
        ICommonService commonService,
        HttpClient httpClient,
        IConfiguration config
    )
    {
        _commonService = commonService;
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<ClientPackageDto> GetClientUsage()
    {
        var clientId = _commonService.GetClientId(); // This returns 4437

        Console.WriteLine($"AccessKey: {ACCESS_KEY}");
        Console.WriteLine($"ApiBaseUrl: {API_BASE_URL}");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            ACCESS_KEY
        );
        var response = await _httpClient.GetAsync(
            $"{API_BASE_URL}/api/SubscriptionPlans/GetSubscriptionPackage?activeId=1&clientId={clientId}"
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting usage data: {response.StatusCode}");
        }

        var packages = await response.Content.ReadFromJsonAsync<List<ClientPackageDto>>();
        return packages?.FirstOrDefault() ?? new ClientPackageDto();
    }
}