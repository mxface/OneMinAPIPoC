using System.Net.Http.Headers;
using System.Net.Http.Json;
using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Models;

namespace OneMinuteIntegrationOfAPI.Services;

public class CommonService : ICommonService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private string _accessKey = string.Empty;
    private const int FIXED_CLIENT_ID = 4512;

    public CommonService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetAccessKey()
    {
        if (!string.IsNullOrEmpty(_accessKey))
            return _accessKey;

        var apiBaseUrl = _config["MxFace:ApiBaseUrl"];
        var adminToken = _config["MxFace:BearerToken"];

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            adminToken
        );
        var response = await _httpClient.GetAsync($"{apiBaseUrl}/api/Client/getclientbyid");

        if (response.IsSuccessStatusCode)
        {
            var clientDetails = await response.Content.ReadFromJsonAsync<ClientModelDetails>();
            _accessKey = clientDetails?.AccessKey ?? string.Empty;
            return _accessKey;
        }

        throw new Exception($"Failed to get access key: {response.StatusCode}");
    }

    public int GetClientId() => FIXED_CLIENT_ID;
}
