using System;
using OneMinuteIntegrationOfAPI.Models;

namespace OneMinuteIntegrationOfAPI.Interfaces;

public interface IServiceUsageHistoryService
{
    Task<ClientPackageDto> GetClientUsage();
}
