#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Models.StatisticModel;
using FTM.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace FTM.Infrastructure.DataAccess.Context;

public class FtmDbContext : DbContext
{
    public DbSet<Issue> Issues { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<BotStatus> BotStatuses { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
    public virtual DbSet<IssuesForDaily> SpIssuesForDailies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_CONNECTION") ?? throw new Exception("Set connection string into DATABASE_CONNECTION environment variable"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IssuesForDaily>().ToFunction("select_today_issues");
    }
}

[Keyless]
public class IssuesForDaily
{
    [Column("userid")]
    public int UserId { get; set; }
    [Column("issueid")]
    public int IssueId { get; set; }
}