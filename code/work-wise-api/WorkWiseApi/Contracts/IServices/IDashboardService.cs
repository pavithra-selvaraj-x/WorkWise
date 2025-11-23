using Entities.Dtos;

namespace Contracts.IServices;

/// <summary>
/// Interface for dashboard management service.
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Retrieve dashboard insights.
    /// </summary>
    /// <param name="userId">The Id of the user for whom the insights are retrieved.</param>
    /// <returns>The DashboardInsightsDto object containing insights.</returns>
    DashboardInsightsDto GetDashboardInsights(Guid userId);

    /// <summary>
    /// Retrieve dashboard info.
    /// </summary>
    /// <param name="userId">The Id of the user for whom the info are retrieved.</param>
    /// <returns>The DashboardInfoDto object containing info.</returns>
    Task<DashboardInfoDto> GetDashboardInfo(Guid userId);
}