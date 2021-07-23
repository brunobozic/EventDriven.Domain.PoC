using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "UserAddress");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "Town");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "County");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "Country");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "CityBlock");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "AddressTypes");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 43, 59, 834, DateTimeKind.Unspecified).AddTicks(5185),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 38, 3, 834, DateTimeKind.Unspecified).AddTicks(414),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "UserAddress",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "Town",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "County",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "Country",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "CityBlock",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "AddressTypes",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ActivatedByUserId",
                "Addresses",
                "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 38, 3, 834, DateTimeKind.Unspecified).AddTicks(414),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 43, 59, 834, DateTimeKind.Unspecified).AddTicks(5185),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}