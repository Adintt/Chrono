using Chrono.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Chrono.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TimeEntry> Times { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().ToTable("Tasks");
            modelBuilder.Entity<TimeEntry>().ToTable("Times");

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Times)
                .WithOne(te => te.Task)
                .HasForeignKey(te => te.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
