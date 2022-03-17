using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class somestuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "UserRoles",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "UserAddress",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "Town",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "County",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "Country",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "CityBlock",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "ApplicationUsers",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "ApplicationRoles",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "AddressTypes",
                "IsDeleted");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                "Addresses",
                "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
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
                    new DateTime(2021, 8, 12, 15, 43, 29, 901, DateTimeKind.Unspecified).AddTicks(224),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 12, 53, 5, 664, DateTimeKind.Unspecified).AddTicks(7591),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Deleted",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Deleted",
                "Addresses");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "UserRoles",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "UserAddress",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "Town",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "County",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "Country",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "CityBlock",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "ApplicationUsers",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "ApplicationRoles",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "AddressTypes",
                "TheUserHasBeenDeleted");

            migrationBuilder.RenameColumn(
                "IsDeleted",
                "Addresses",
                "TheUserHasBeenDeleted");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 12, 53, 5, 664, DateTimeKind.Unspecified).AddTicks(7591),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 8, 12, 15, 43, 29, 901, DateTimeKind.Unspecified).AddTicks(224),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}