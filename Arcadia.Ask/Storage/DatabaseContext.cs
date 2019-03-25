﻿namespace Arcadia.Ask.Storage
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Models.Entities;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<QuestionEntity> Questions { get; set; }

        public DbSet<VoteEntity> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoteEntity>()
                .HasKey(v => new {v.QuestionId, v.UserId});

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
    }
}