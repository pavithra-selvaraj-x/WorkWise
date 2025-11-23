using Entities.Dtos;

namespace Contracts.IServices;

/// <summary>
/// Interface for goal management service.
/// </summary>
public interface IGoalManagementService
{
    /// <summary>
    /// Get goal suggestions based on the provided details.
    /// </summary>
    /// <param name="goalDetailsDto">The details of the user's goal, including goal, importance, time frame, and time per week.</param>
    /// <returns>A collection of goal suggestions.</returns>
    Task<GoalSuggestionsDto> GetGoalSuggestions(GoalDetailsDto goalDetailsDto);

    /// <summary>
    /// Create goal.
    /// </summary>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <param name="userId">The id of the user.</param>
    ResponseDto CreateGoal(GoalDto goalDto, Guid userId);

    /// <summary>
    /// Delete the goal by ID.
    /// </summary>
    /// <param name="goalId">ID of the goal to be deleted.</param>
    /// <param name="userId">The id of the user.</param>
    void DeleteGoal(Guid goalId, Guid userId);

    /// <summary>
    /// Update Goal By Id.
    /// </summary>
    /// <param name="goalId">ID of the goal to be updated.</param>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <param name="userId">The id of the user.</param>
    void UpdateGoal(Guid goalId, GoalDto goalDto, Guid userId);

    /// <summary>
    /// Get all goals for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of goal DTOs.</returns>
    List<GoalDto> GetAllGoals(Guid userId);

    /// <summary>
    /// Get goal by Id
    /// </summary>
    /// <param name="goalId">The unique identifier of the goal.</param>
    /// <param name="loggedInUserId">The unique identifier of the user.</param>
    /// <returns>The goalDto object</returns>
    GoalDto GetGoalById(Guid goalId, Guid userId);

    /// <summary>
    /// This method calls the open API for the given masked code and 
    /// </summary>
    /// <param name="prompt">The Prompt for Open AI</param>
    /// <param name="userMessage">The user goal related message</param>
    /// <returns>Returns the response from the Open AI</returns>
    Task<GeminiResponseDto> GetResponseFromGenAIModel(string prompt);
}