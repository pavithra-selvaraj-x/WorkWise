using System.Runtime.Serialization;

namespace Entities.Dtos;

/// <summary>
/// Dashboard Info DTO containing insights for user.
/// </summary>
public class DashboardInfoDto
{
    /// <summary>
    /// A list of insights for the user's dashboard.
    /// </summary>
    [DataMember(Name = "insights")]
    public List<string> Insights { get; set; }

    /// <summary>
    /// A list of motivations for the user's dashboard.
    /// </summary>
    [DataMember(Name = "motivations")]
    public List<string> Motivations { get; set; }
}