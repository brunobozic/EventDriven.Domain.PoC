using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "Town");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "County");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "CityBlock");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "ApplicationRoles");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "AddressTypes");

            migrationBuilder.DropColumn(
                name: "ActivatedByUserId",
                table: "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 8, 43, 59, 834, DateTimeKind.Unspecified).AddTicks(5185), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 8, 38, 3, 834, DateTimeKind.Unspecified).AddTicks(414), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "UserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "UserAddress",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "Town",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "County",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "Country",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "CityBlock",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "ApplicationUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "ApplicationRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "AddressTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByUserId",
                table: "Addresses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 8, 38, 3, 834, DateTimeKind.Unspecified).AddTicks(414), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 7, 12, 8, 43, 59, 834, DateTimeKind.Unspecified).AddTicks(5185), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
