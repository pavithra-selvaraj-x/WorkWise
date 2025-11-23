namespace Entities.Dtos;

public class ContentPart
{
    public string Text { get; set; }
}

public class Content
{
    public List<ContentPart> Parts { get; set; }
}

public class Candidate
{
    public Content Content { get; set; }
}

public class PromptFeedback
{
    public List<object> SafetyRatings { get; set; }
}

public class GeminiResponseDto
{
    public List<Candidate> Candidates { get; set; }
    public PromptFeedback PromptFeedback { get; set; }
}