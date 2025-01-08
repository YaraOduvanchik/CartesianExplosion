using CartesianExplosion.Entities;

namespace CartesianExplosion.DataBase;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<SubIssue> SubIssues { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ChangeHistory> ChangeHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Issue>()
            .HasOne(i => i.Project)
            .WithMany(p => p.Issues)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SubIssue>()
            .HasOne(si => si.Issue)
            .WithMany(i => i.SubIssues)
            .HasForeignKey(si => si.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.SubIssue)
            .WithMany(si => si.Comments)
            .HasForeignKey(c => c.SubIssueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChangeHistory>()
            .HasOne(ch => ch.SubIssue);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    }
}