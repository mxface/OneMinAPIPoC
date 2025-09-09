using System.Net.Http.Headers;
using System.Net.Http.Json;
using OneMinuteIntegrationOfAPI.Interfaces;
using OneMinuteIntegrationOfAPI.Models;

namespace OneMinuteIntegrationOfAPI.Services;

public class CommonService : ICommonService
{
    private readonly HttpClient _httpClient;
    private const int FIXED_CLIENT_ID = 4437;

    public CommonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public int GetClientId() => FIXED_CLIENT_ID;
}
