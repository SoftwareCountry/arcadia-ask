using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arcadia.Ask.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Login", "Hash" },
                values: new object[] { "admin", "AQAAAAEAACcQAAAAEOoGxBJ4DDHCDmmh9dETHA2CzQnkdtA4Fc+9ZkUVGVqhkfl48+Nv3t/KaGCmVGnzRA==" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserLogin", "RoleId" },
                values: new object[] { "admin", new Guid("af2a4f78-3712-4300-abcd-d59a0136c833") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserLogin", "RoleId" },
                values: new object[] { "admin", new Guid("835f137e-4b44-4e7b-8563-849eb151fd74") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "UserLogin", "RoleId" },
                keyValues: new object[] { "admin", new Guid("835f137e-4b44-4e7b-8563-849eb151fd74") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "UserLogin", "RoleId" },
                keyValues: new object[] { "admin", new Guid("af2a4f78-3712-4300-abcd-d59a0136c833") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin");
        }
    }
}
