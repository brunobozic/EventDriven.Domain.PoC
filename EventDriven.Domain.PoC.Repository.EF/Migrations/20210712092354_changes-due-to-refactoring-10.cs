using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "UserRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "UserAddress",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "Town",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "County",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "Country",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "CityBlock",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "ApplicationUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "ApplicationRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "AddressTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeed",
                table: "Addresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 9, 12, 24, 88, DateTimeKind.Unspecified).AddTicks(630), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "Town");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "County");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "CityBlock");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "ApplicationRoles");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "AddressTypes");

            migrationBuilder.DropColumn(
                name: "IsSeed",
                table: "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 9, 12, 24, 88, DateTimeKind.Unspecified).AddTicks(630), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
