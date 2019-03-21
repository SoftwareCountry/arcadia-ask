using Arcadia.Ask.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Arcadia.Ask.Storage
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionEntity>().HasData(
                new QuestionEntity
                {
                    QuestionId = Guid.NewGuid(),
                    Text = "Test",
                    Author = "Author",
                    IsApproved = true,
                    PostedAt = DateTimeOffset.Now
                }
            );
        }

        public DbSet<QuestionEntity> Questions { get; set; }

        public DbSet<VoteEntity> Votes { get; set; }
    }
}
