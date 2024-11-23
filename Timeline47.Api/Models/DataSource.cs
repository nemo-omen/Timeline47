using System.ComponentModel.DataAnnotations;

namespace Timeline47.Api.Models;

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
    [MaxLength(255)]
    public string? Category { get; set; }
}