using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "UserAddress",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "Town",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "County",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "Country",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "CityBlock",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "AddressTypes",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSeed",
                "Addresses",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 12, 24, 88, DateTimeKind.Unspecified).AddTicks(630),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "IsSeed",
                "UserRoles");

            migrationBuilder.DropColumn(
                "IsSeed",
                "UserAddress");

            migrationBuilder.DropColumn(
                "IsSeed",
                "Town");

            migrationBuilder.DropColumn(
                "IsSeed",
                "County");

            migrationBuilder.DropColumn(
                "IsSeed",
                "Country");

            migrationBuilder.DropColumn(
                "IsSeed",
                "CityBlock");

            migrationBuilder.DropColumn(
                "IsSeed",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "IsSeed",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "IsSeed",
                "AddressTypes");

            migrationBuilder.DropColumn(
                "IsSeed",
                "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 12, 24, 88, DateTimeKind.Unspecified).AddTicks(630),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}