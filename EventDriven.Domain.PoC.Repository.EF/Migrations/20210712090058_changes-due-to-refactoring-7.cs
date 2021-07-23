using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 0, 58, 5, DateTimeKind.Unspecified).AddTicks(7199),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 54, 4, 368, DateTimeKind.Unspecified).AddTicks(1244),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 8, 54, 4, 368, DateTimeKind.Unspecified).AddTicks(1244),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 0, 58, 5, DateTimeKind.Unspecified).AddTicks(7199),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}