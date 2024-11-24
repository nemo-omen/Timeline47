using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Timeline47.Api.Data;
using Timeline47.Api.Models;

namespace Timeline47.Api.Shared;

sealed class JobStorageProvider : IJobStorageProvider<JobRecord>, IJobResultProvider
{
    readonly PooledDbContextFactory<ApplicationDbContext> _dbPool;

    public JobStorageProvider()
    {
        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .UseNpgsql($"Host=localhost;Port=5432;Database=Tl47_Dev;Username=Tl47Admin;Password=Password123!").Options;
        _dbPool = new(opts);
        using var db = _dbPool.CreateDbContext();
    }

    public async Task StoreJobAsync(JobRecord job, CancellationToken ct)
    {
        using var db = _dbPool.CreateDbContext();
        await db.AddAsync(job, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        using var db = _dbPool.CreateDbContext();

        return await db.Jobs
                       .Where(p.Match)
                       .Take(p.Limit)
                       .ToListAsync(p.CancellationToken);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        if (job is not null)
        {
            job.IsComplete = true;
            db.Update(job);
            await db.SaveChangesAsync(c);
        }
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord job, Exception e, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        job.ExecuteAfter = DateTime.UtcNow.AddMinutes(1);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        using var db = _dbPool.CreateDbContext();
        var staleJobs = db.Jobs.Where(p.Match);
        db.RemoveRange(staleJobs);
        await db.SaveChangesAsync(p.CancellationToken);
    }

    public async Task StoreJobResultAsync<TResult>(Guid trackingId, TResult result, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        var job = await db.Jobs.SingleAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        ((IJobResultStorage)job).SetResult(result);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task<TResult?> GetJobResultAsync<TResult>(Guid trackingId, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        return job is not null
                   ? ((IJobResultStorage)job).GetResult<TResult>()
                   : default;
    }
}