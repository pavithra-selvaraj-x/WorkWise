using Contracts;
using Contracts.IRepository;
using Contracts.IServices;
using Entities.Dtos;
using Entities.Models;
using ExceptionHandler;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Services;

public class GoalManagementService : IGoalManagementService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repoWrapper;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for injecting repoWrapper, logger and configuration
    /// </summary>
    /// <param name="repoWrapper">The repoWrapper object</param>
    /// <param name="logger">The logger object</param>
    /// <param name="configuration"></param>
    public GoalManagementService(IRepositoryWrapper repoWrapper, ILoggerManager logger, IConfiguration configuration)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Get goal suggestions based on the provided details.
    /// </summary>
    /// <param name="goalDetailsDto">The details of the user's goal, including goal, importance, time frame, and time per week.</param>
    /// <returns>A collection of goal suggestions.</returns>
    public async Task<GoalSuggestionsDto> GetGoalSuggestions(GoalDetailsDto goalDetailsDto)
    {
        try
        {
            _logger.LogDebug("Entering the GetGoalSuggestions method.");
            string prompt = $"You are developing an application for goal management, and a user has just submitted some information about their goal. Here's a summary of the user's input:\n\n" +
                              $"1. **Goal**: {goalDetailsDto.Goal}\n" +
                              $"2. **Importance**: {goalDetailsDto.Importance}\n" +
                              $"3. **Timeframe**: {goalDetailsDto.TimeFrame}\n" +
                              $"4. **Time per Week**: {goalDetailsDto.TimePerWeek} hours\n\n" +
                              $"Based on this information, your task is to generate a list of SMART goal tasks. SMART goals are Specific, Measurable, Achievable, Relevant, and Time-bound. Consider the user's input to generate actionable tasks that align with their goal and timeframe. Once generated, provide the user with a list of suggested tasks to help them achieve their goal effectively in parsable json format only.\n\n" +
                              $"The response should be a list of tasks represented by the following properties:\n" +
                              $"- Title: A short title of the task\n" +
                              $"- Description: A brief description of the task\n" +
                              $"- Priority: The priority level of the task\n" +
                              $"- Deadline: The deadline by which the task should be completed in the ISO 8601 format";

            GeminiResponseDto result = await GetResponseFromGenAIModel(prompt);

            // Access text part
            string text = result.Candidates[0].Content.Parts[0].Text;
            string jsonText = text.Replace("```json", "").Replace("```", "");
            Console.WriteLine(jsonText);
            List<GoalTask> goalTasks = JsonConvert.DeserializeObject<List<GoalTask>>(jsonText);
            Console.WriteLine(goalTasks);
            GoalSuggestionsDto goalSuggestionsDto = new GoalSuggestionsDto
            {
                Tasks = goalTasks
            };
            _logger.LogDebug("Exiting the GetGoalSuggestions method.");
            return goalSuggestionsDto;
        }
        catch (Exception)
        {
            _logger.LogError("Exception occurred in GetSuggestedResponse");
            throw;
        }
    }

    /// <summary>
    /// This method calls the open API for the given masked code and 
    /// </summary>
    /// <param name="prompt">The Prompt for Open AI</param>
    /// <param name="userMessage">The user goal related message</param>
    /// <returns>Returns the response from the Open AI</returns>
    public async Task<GeminiResponseDto> GetResponseFromGenAIModel(string prompt)
    {
        try
        {
            var apiClient = new ApiClient();
            string baseUrl = _configuration["GenAI:BaseUrl"];
            string apiKey = _configuration["GenAI:ApiKey"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                _logger.LogError("GenAI:BaseUrl is not configured.");
                throw new InvalidOperationException("GenAI:BaseUrl is not configured.");
            }

            string apiUrl = string.IsNullOrWhiteSpace(apiKey) ? baseUrl : $"{baseUrl}?key={apiKey}";
            string requestBody = $"{{\"contents\": [{{\"parts\": [{{\"text\": \"{prompt}\"}}]}}]}}";
            GeminiResponseDto response = apiClient.PostRequestAsync<GeminiResponseDto>(apiUrl, requestBody);
            return response;
        }
        catch (Exception)
        {
            _logger.LogError("Exception occurred in GetResponseFromOpenAI");
            throw;
        }
    }

    /// <summary>
    /// Create goal.
    /// </summary>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <param name="userId">The id of the user.</param>
    public ResponseDto CreateGoal(GoalDto goalDto, Guid userId)
    {
        _logger.LogInfo("Creating goal with the given details");
        Goal goal = MapGoalDtoToEntity(goalDto, userId);
        List<Entities.Models.Task> tasks = MapTaskDtosToEntities(goalDto.TaskList, userId, goal.Id);

        _repoWrapper.Goal.Create(goal);
        _repoWrapper.Task.CreateBatch(tasks);
        _repoWrapper.Save();

        ResponseDto response = new ResponseDto
        {
            Id = goal.Id
        };
        return response;
    }

    private Goal MapGoalDtoToEntity(GoalDto goalDto, Guid userId)
    {
        return new Goal
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = goalDto.Title,
            Description = goalDto.Description,
            StartDate = goalDto.StartDate,
            EndDate = goalDto.EndDate,
            Status = goalDto.Status,
            Priority = goalDto.Priority,
            Progress = goalDto.Progress
        };
    }

    private List<Entities.Models.Task> MapTaskDtosToEntities(List<TaskDto>? taskDtos, Guid userId, Guid goalId)
    {
        if (taskDtos == null)
        {
            return new List<Entities.Models.Task>();
        }

        return taskDtos.Select(taskDto => new Entities.Models.Task
        {
            UserId = userId,
            GoalId = goalId,
            Title = taskDto.Title,
            Description = taskDto.Description,
            StartDate = taskDto.StartDate,
            EndDate = taskDto.EndDate,
            DueDate = taskDto.DueDate,
            Status = taskDto.Status,
            Priority = taskDto.Priority,
            TaskType = taskDto.TaskType
        }).ToList();
    }

    /// <summary>
    /// Delete the goal by ID.
    /// </summary>
    /// <param name="goalId">ID of the goal to be deleted.</param>
    /// <param name="userId">The id of the user.</param>
    public void DeleteGoal(Guid goalId, Guid userId)
    {
        _logger.LogInfo($"Deleting Goal with Id: {goalId}");
        Goal goal = _repoWrapper.Goal.FindGoalByIdAndUserId(goalId, userId);

        if (goal == null)
        {
            _logger.LogError($"Goal with ID {goalId} not found for user {userId}");
            throw new NotFoundCustomException("Goal not found.", "Goal not found for the given ID.");
        }

        goal.IsActive = false;
        _repoWrapper.Goal.Update(goal);

        List<Entities.Models.Task> tasks = _repoWrapper.Task.FindTasksByGoalId(goalId);
        if (tasks.Any())
        {
            tasks.ForEach(task =>
            {
                task.IsActive = false;
            });
            _repoWrapper.Task.UpdateBatch(tasks);
        }

        _repoWrapper.Save();
    }

    /// <summary>
    /// Update Goal By Id.
    /// </summary>
    /// <param name="goalId">ID of the goal to be updated.</param>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <param name="userId">The id of the user.</param>
    public void UpdateGoal(Guid goalId, GoalDto goalDto, Guid userId)
    {
        _logger.LogInfo($"Updating Goal with Id: {goalId}");
        Goal goal = _repoWrapper.Goal.FindGoalByIdAndUserId(goalId, userId);

        if (goal == null)
        {
            _logger.LogError($"Goal with ID {goalId} not found for user {userId}");
            throw new NotFoundCustomException("Goal not found.", "Goal not found for the given ID.");
        }

        goal.Title = goalDto.Title;
        goal.Description = goalDto.Description;
        goal.StartDate = goalDto.StartDate;
        goal.EndDate = goalDto.EndDate;
        goal.Status = goalDto.Status;
        goal.Priority = goalDto.Priority;

        _repoWrapper.Goal.Update(goal);
        _repoWrapper.Save();
    }

    /// <summary>
    /// Get all goals for the user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of goal DTOs.</returns>
    public List<GoalDto> GetAllGoals(Guid userId)
    {
        return _repoWrapper.Goal.GetAllGoalsByUserId(userId);
    }

    /// <summary>
    /// Get goal by Id
    /// </summary>
    /// <param name="goalId">The unique identifier of the goal.</param>
    /// <param name="loggedInUserId">The unique identifier of the user.</param>
    /// <returns>The goalDto object</returns>
    public GoalDto GetGoalById(Guid goalId, Guid userId)
    {
        GoalDto? goal = _repoWrapper.Goal.GetGoalById(goalId, userId);
        if (goal == null)
        {
            _logger.LogError($"Goal with ID {goalId} not found for user {userId}");
            throw new NotFoundCustomException("Goal not found.", "Goal not found for the given ID.");
        }
        return goal;
    }
}