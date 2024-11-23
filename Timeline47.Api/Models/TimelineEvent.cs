using System.ComponentModel.DataAnnotations;

namespace Timeline47.Api.Models;

public class TimelineEvent : BaseModel
{
    [MaxLength(255)]
    public required string Title { get; set; }
    [MaxLength(1023)]
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Subject> Subjects { get; set; } = [];
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Article> Articles { get; set; } = [];
}