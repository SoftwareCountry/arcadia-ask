namespace Arcadia.Ask.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Arcadia.Ask.Auth;
    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.Entities;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        private readonly IPasswordHasher<User> passwordHasher;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IPasswordHasher<User> passwordHasher) : base(options)
        {
            this.passwordHasher = passwordHasher;
        }

        public DbSet<QuestionEntity> Questions { get; set; }

        public DbSet<VoteEntity> Votes { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoteEntity>()
                .HasKey(v => new { v.QuestionId, v.UserId });

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

            var userRole = new RoleEntity()
            {
                RoleId = Guid.Parse("af2a4f78-3712-4300-abcd-d59a0136c833"),
                Name = RoleNames.User
            };

            var moderatorRole = new RoleEntity()
            {
                RoleId = Guid.Parse("835f137e-4b44-4e7b-8563-849eb151fd74"),
                Name = RoleNames.Moderator
            };

            modelBuilder.Entity<RoleEntity>().HasData(userRole, moderatorRole);

            var adminRoles = new[] { userRole, moderatorRole };

            var adminLogin = "admin";
            var adminPassword = "changeit";
            
            this.AddUser(modelBuilder, adminLogin, adminRoles, adminPassword);
        }

        private void AddUser(ModelBuilder modelBuilder, string login, RoleEntity[] roles, string password)
        {
            var adminUser = new User(login, roles.Select(x => x.Name));

            var admin = new UserEntity()
            {
                Hash = this.passwordHasher.HashPassword(adminUser, password),
                Login = login
            };

            modelBuilder.Entity<UserEntity>().HasData(admin);
            modelBuilder.Entity<UserRoleEntity>().HasData(roles.Select(x => new UserRoleEntity() { RoleId = x.RoleId, UserLogin = admin.Login }));
        }
    }
}