using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class dontknowwhat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Addresses");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2022, 10, 21, 11, 37, 59, 333, DateTimeKind.Unspecified).AddTicks(2650), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 8, 12, 15, 43, 29, 901, DateTimeKind.Unspecified).AddTicks(224), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Addresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 8, 12, 15, 43, 29, 901, DateTimeKind.Unspecified).AddTicks(224), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2022, 10, 21, 11, 37, 59, 333, DateTimeKind.Unspecified).AddTicks(2650), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}