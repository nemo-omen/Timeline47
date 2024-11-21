using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timeline47.Models;

namespace Timeline47.Data;

// public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() {}
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Article> Articles { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectType> SubjectTypes { get; set; }
    public DbSet<ArticleSubject> ArticleSubjects { get; set; }
    public DbSet<NewsSource> NewsSources { get; set; }
    public DbSet<DataSource> DataSources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>()
            .HasMany(a => a.Subjects)
            .WithMany(s => s.Articles)
            .UsingEntity<ArticleSubject>();
    }
}