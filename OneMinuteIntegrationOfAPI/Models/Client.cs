using System;

namespace OneMinuteIntegrationOfAPI.Models;

public class Client
{
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string SubscriptionKey { get; set; } = string.Empty;
}
