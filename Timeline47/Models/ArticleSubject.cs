namespace Timeline47.Models;

public class ArticleSubject : BaseModel
{
    public Guid ArticleId { get; set; }
    public Guid SubjectId { get; set; }
}