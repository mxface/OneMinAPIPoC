using System;

namespace OneMinuteIntegrationOfAPI.Models;

public class ClientModelDetails
{
    public int? ParentClientId { get; set; }
    public string AccessKey { get; set; } = string.Empty;
    public int Country { get; set; }
    public int ServiceType { get; set; }
    public int status_code { get; set; }
    public sbyte? Enable1ToN { get; set; }
    public sbyte? DocRead { get; set; }
    public sbyte? IsRecurring { get; set; }
    public string ClientPhone { get; set; } = string.Empty;
}
