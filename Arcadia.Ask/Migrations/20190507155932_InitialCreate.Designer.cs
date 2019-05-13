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
    [Migration("20190507155932_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Arcadia.Ask.Models.Entities.ModeratorEntity", b =>
                {
                    b.Property<string>("Login")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hash");

                    b.HasKey("Login");

                    b.ToTable("Moderators");
                });

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

                    b.HasData(
                        new
                        {
                            QuestionId = new Guid("7c3de3b3-2266-439a-bd18-0b23d3001b4f"),
                            Author = "Author",
                            IsApproved = true,
                            PostedAt = new DateTimeOffset(new DateTime(2019, 5, 7, 18, 59, 31, 285, DateTimeKind.Unspecified).AddTicks(1942), new TimeSpan(0, 3, 0, 0, 0)),
                            Text = "Test"
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
