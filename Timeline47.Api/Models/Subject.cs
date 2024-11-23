using System.ComponentModel.DataAnnotations;

namespace Timeline47.Api.Models;

public class Subject : BaseModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    public required SubjectType Type { get; set; }
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Article> Articles { get; set; } = [];
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<TimelineEvent> Events { get; set; } = [];
}