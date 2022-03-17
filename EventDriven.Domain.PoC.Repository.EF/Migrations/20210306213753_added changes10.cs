using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateDeleted",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 6, 21, 37, 52, 577, DateTimeKind.Unspecified).AddTicks(2440),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 6, 21, 37, 52, 569, DateTimeKind.Unspecified).AddTicks(376),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                "EmailActedUpon",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "UserNameActedUpon",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "DateCreated",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "EmailActedUpon",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "UserNameActedUpon",
                "AccountJournalEntry");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateDeleted",
                "AccountJournalEntry",
                "TEXT",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 6, 21, 37, 52, 577, DateTimeKind.Unspecified).AddTicks(2440),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}