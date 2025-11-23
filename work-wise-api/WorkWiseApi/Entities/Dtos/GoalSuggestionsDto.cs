using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Entities.Dtos
{
    /// <summary>
    /// The GoalSuggestionsDto used to represent the details of suggestions based on goal.
    /// </summary>
    public class GoalSuggestionsDto
    {
        public List<GoalTask> Tasks { get; set; }
    }

    public class GoalTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Deadline { get; set; }
    }
}