using Timeline47.Api.Data;

namespace Timeline47.Api.Features.ArticleHandling;
public interface IArticleRepository
{
    
}

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;
    
    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}