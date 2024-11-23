namespace Timeline47.Api.Models;

public class Timeline : BaseModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<TimelineEvent> Events { get; set; } = [];
}