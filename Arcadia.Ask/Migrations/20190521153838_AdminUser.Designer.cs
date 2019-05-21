﻿// <auto-generated />
using System;
using Arcadia.Ask.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Arcadia.Ask.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190521153838_AdminUser")]
    partial class AdminUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.QuestionEntity", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<bool>("IsApproved");

                    b.Property<DateTimeOffset>("PostedAt");

                    b.Property<string>("Text");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.RoleEntity", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("af2a4f78-3712-4300-abcd-d59a0136c833"),
                            Name = "User"
                        },
                        new
                        {
                            RoleId = new Guid("835f137e-4b44-4e7b-8563-849eb151fd74"),
                            Name = "Moderator"
                        });
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.UserEntity", b =>
                {
                    b.Property<string>("Login")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hash");

                    b.HasKey("Login");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Login = "admin",
                            Hash = "AQAAAAEAACcQAAAAEOoGxBJ4DDHCDmmh9dETHA2CzQnkdtA4Fc+9ZkUVGVqhkfl48+Nv3t/KaGCmVGnzRA=="
                        });
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.UserRoleEntity", b =>
                {
                    b.Property<string>("UserLogin");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserLogin", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserLogin = "admin",
                            RoleId = new Guid("af2a4f78-3712-4300-abcd-d59a0136c833")
                        },
                        new
                        {
                            UserLogin = "admin",
                            RoleId = new Guid("835f137e-4b44-4e7b-8563-849eb151fd74")
                        });
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.VoteEntity", b =>
                {
                    b.Property<Guid>("QuestionId");

                    b.Property<Guid>("UserId");

                    b.Property<bool>("IsUpvoted");

                    b.HasKey("QuestionId", "UserId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.UserRoleEntity", b =>
                {
                    b.HasOne("Arcadia.Ask.Models.Entities.RoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Arcadia.Ask.Models.Entities.UserEntity", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserLogin")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.VoteEntity", b =>
                {
                    b.HasOne("Arcadia.Ask.Models.Entities.QuestionEntity", "Question")
                        .WithMany("Votes")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
