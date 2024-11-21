using System.ComponentModel.DataAnnotations;

namespace Timeline47.Models;

public class DataSource : BaseModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public string? ShortName { get; set; }
    [MaxLength(255)]
    public required string Url { get; set; }
    [MaxLength(255)]
    public string? DataUrl { get; set; }
}