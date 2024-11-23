using System.ComponentModel.DataAnnotations;

namespace Timeline47.Api.Models;

public class SubjectType : BaseModel
{
    [MaxLength(255)]
    public required string Type { get; set; }
}