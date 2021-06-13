using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "RoleId",
                "ApplicationRoles");

            migrationBuilder.AddColumn<bool>(
                "Active",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveFrom",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveTo",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "RefreshToken",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateDeleted",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateModified",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                "Description",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "LastModifiedBy",
                "RefreshToken",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                "Name",
                "RefreshToken",
                "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Active",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ActiveTo",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "DateCreated",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "DateDeleted",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "DateModified",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "Deleted",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "Description",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "IsDraft",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "Name",
                "RefreshToken");

            migrationBuilder.AddColumn<Guid>(
                "RoleId",
                "ApplicationRoles",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}