using System.ComponentModel.DataAnnotations;

namespace Timeline47.Api.Models;

public class NewsSource : BaseModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public required string Url { get; set; }
    [MaxLength(255)]
    public string? FeedUrl { get; set; }
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Article> Articles { get; set; } = [];
}