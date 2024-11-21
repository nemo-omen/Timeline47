using System.ComponentModel.DataAnnotations;

namespace Timeline47.Models;

public class Subject : BaseModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    public required SubjectType Type { get; set; }
    public ICollection<Article> Articles { get; set; } = [];
}