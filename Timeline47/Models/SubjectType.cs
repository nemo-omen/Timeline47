using System.ComponentModel.DataAnnotations;

namespace Timeline47.Models;

public class SubjectType : BaseModel
{
    [MaxLength(255)]
    public required string Type { get; set; }
}