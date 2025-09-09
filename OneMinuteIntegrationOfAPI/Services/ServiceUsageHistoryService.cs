using System.Net.Http.Headers;
using System.Net.Http.Json;
using OneMinuteIntegrationOfAPI.Constants;
using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Models;

public class ServiceUsageHistoryService : IServiceUsageHistoryService
{
    private readonly ICommonService _commonService;
    private readonly HttpClient _httpClient;

    public ServiceUsageHistoryService(ICommonService commonService, HttpClient httpClient)
    {
        _commonService = commonService;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            SharedConstant.BEARER_TOKEN
        );
    }

    public async Task<ClientPackageDto> GetClientUsage()
    {
        try
        {
            var clientId = _commonService.GetClientId();
            var response = await _httpClient.GetAsync(
                $"{SharedConstant.API_BASE_URL}/api/SubscriptionPlans/GetSubscriptionPackage?activeId=1&clientId={clientId}"
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error getting usage data: {response.StatusCode}");
            }

            var packages = await response.Content.ReadFromJsonAsync<List<ClientPackageDto>>();
            return packages?.FirstOrDefault() ?? new ClientPackageDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetClientUsage: {ex.Message}");
            return new ClientPackageDto();
        }
    }
}
