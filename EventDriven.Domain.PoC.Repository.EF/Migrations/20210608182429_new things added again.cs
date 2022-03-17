using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class newthingsaddedagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Id2",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Id2",
                "UserAddress");

            migrationBuilder.DropColumn(
                "Id2",
                "Town");

            migrationBuilder.DropColumn(
                "Id2",
                "County");

            migrationBuilder.DropColumn(
                "Id2",
                "Country");

            migrationBuilder.DropColumn(
                "Id2",
                "CityBlock");

            migrationBuilder.DropColumn(
                "Id2",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "Id2",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Id2",
                "AddressTypes");

            migrationBuilder.DropColumn(
                "Id2",
                "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 6, 8, 18, 24, 28, 718, DateTimeKind.Unspecified).AddTicks(7108),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 6, 4, 11, 7, 2, 731, DateTimeKind.Unspecified).AddTicks(1190),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "Id2",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "UserAddress",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "Town",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "County",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "Country",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "CityBlock",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "AddressTypes",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "Addresses",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 6, 4, 11, 7, 2, 731, DateTimeKind.Unspecified).AddTicks(1190),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 6, 8, 18, 24, 28, 718, DateTimeKind.Unspecified).AddTicks(7108),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}