using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TaskTag> TaskTags => Set<TaskTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many-to-Many (Task <-> Tag)
        modelBuilder.Entity<TaskTag>()
            .HasKey(tt => new { tt.TaskItemId, tt.TagId });

        modelBuilder.Entity<TaskTag>()
            .HasOne(tt => tt.TaskItem)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TaskItemId);

        modelBuilder.Entity<TaskTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TagId);
    }
}
