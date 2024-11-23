using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timeline47.Api.Models;

namespace Timeline47.Api.Data;

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
    public DbSet<TimelineEvent> TimelineEvents { get; set; }
    public DbSet<NewsSource> NewsSources { get; set; }
    public DbSet<DataSource> DataSources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}