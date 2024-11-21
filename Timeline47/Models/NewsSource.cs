using System.ComponentModel.DataAnnotations;

namespace Timeline47.Models;

public class NewsSource : BaseModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public required string Url { get; set; }
    [MaxLength(255)]
    public string? FeedUrl { get; set; }
    public ICollection<Article> Articles { get; set; } = [];
}