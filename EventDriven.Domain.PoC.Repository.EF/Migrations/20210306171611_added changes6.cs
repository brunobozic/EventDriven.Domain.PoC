using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreateddBy",
                "ApplicationRoles");

            migrationBuilder.AddColumn<Guid>(
                "UserIdGuid",
                "ApplicationUsers",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                "RoleIdGuid",
                "ApplicationRoles",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "UserIdGuid",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "RoleIdGuid",
                "ApplicationRoles");

            migrationBuilder.AddColumn<long>(
                "CreateddBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);
        }
    }
}