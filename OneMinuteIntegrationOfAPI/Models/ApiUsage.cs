using System;

namespace OneMinuteIntegrationOfAPI.Models;

public class ApiUsage
{
    public string ApiName { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public int UsageCount { get; set; }
    public DateTime UsageDate { get; set; }
}
