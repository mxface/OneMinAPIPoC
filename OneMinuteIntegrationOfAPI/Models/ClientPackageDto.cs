using System;

namespace OneMinuteIntegrationOfAPI.Models;

public class ClientPackageDto
{
    public string packageName { get; set; } = "";
    public sbyte? active { get; set; }
    public int totalUsage { get; set; }
    public int remainingUsage { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
}
