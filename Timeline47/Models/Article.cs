using System.ComponentModel.DataAnnotations;

namespace Timeline47.Models;

public class Article : BaseModel
{
    [MaxLength(255)]
    public required string Title { get; set; }
    [MaxLength(1023)]
    public required string Summary { get; set; }
    public DateTime PublicationDate { get; set; }
    [MaxLength(255)]
    public required string Url { get; set; }
    public Guid NewsSourceId { get; set; }
    public required NewsSource NewsSource { get; set; }
    public ICollection<Subject> Subjects { get; set; } = [];
}