namespace Arcadia.Ask.Storage
{
    using System;

    using Arcadia.Ask.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoteEntity>()
                .HasKey(v => new { v.QuestionId, v.UserId });

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

        public DbSet<ModeratorEntity> Moderators { get; set; }
    }
}