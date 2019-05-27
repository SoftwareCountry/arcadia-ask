using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcadia.Ask.Migrations
{
    public partial class VoteUserIdTypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Votes",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                columns: new []{"QuestionId", "UserId"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Votes",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                columns: new[] { "QuestionId", "UserId" });
        }
    }
}
