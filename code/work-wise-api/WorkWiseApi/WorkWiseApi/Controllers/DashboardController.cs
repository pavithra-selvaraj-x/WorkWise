using System.ComponentModel.DataAnnotations;
using Contracts;
using Contracts.IServices;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WorkWiseApi.Attributes;

namespace WorkWiseApi.Controllers;

/// <summary>
/// Class <c>DashboardController</c> for managing the dashboard operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ILoggerManager _logger;

    private readonly IDashboardService _dashboardService;

    /// <summary>
    /// Constructor for inject the dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dashboardService"></param>
    public DashboardController(ILoggerManager logger, IDashboardService dashboardService)
    {
        _logger = logger;
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Get insights related to goals and tasks for the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// Retrieves dashboard insights such as total count, count and percentage grouped by status,
    /// count and percentage grouped by priority for tasks and goals
    /// </remarks>
    /// <returns>A DashboardDataDto object containing the goal and task insights.</returns>
    [HttpGet("/api/dashboard/goal-dashboard-insights")]
    [Authorize]
    [SwaggerOperation("GetDashboardInsights")]
    [SwaggerResponse(statusCode: 200, type: typeof(DashboardInsightsDto), description: "Dashboard insights retrieved successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult GetGoalInsights()
    {
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _logger.LogDebug($"Fetching dashboard insights for the user {loggedInUserId}");
        DashboardInsightsDto dashboardInsights = _dashboardService.GetDashboardInsights(loggedInUserId);
        return StatusCode(200, dashboardInsights);
    }

    /// <summary>
    /// Get stats and insights for the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// Retrieves stats and insights for the user based on goal and task data.
    /// </remarks>
    /// <returns>An DashboardInfoDto object containing the stats and insights.</returns>
    [HttpGet("/api/dashboard/dashboard-info")]
    [Authorize]
    [SwaggerOperation("GetDashboardInfo")]
    [SwaggerResponse(statusCode: 200, type: typeof(DashboardInfoDto), description: "Dashboard info retrieved successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult GetDashboardInfo()
    {
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _logger.LogDebug($"Fetching dashboard info for the user {loggedInUserId}");
        DashboardInfoDto dashboardInfo = _dashboardService.GetDashboardInfo(loggedInUserId).Result;
        return StatusCode(200, dashboardInfo);
    }
}