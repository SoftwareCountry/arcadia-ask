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

            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(ur => new { ur.UserLogin, ur.RoleId });

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserLogin);

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

        public DbSet<QuestionEntity> Questions { get; set; }

        public DbSet<VoteEntity> Votes { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }
    }
}