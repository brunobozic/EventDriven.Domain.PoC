using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedenqueuedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                "EnqueueDate",
                "InternalCommands",
                "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 10, 10, 21, 8, 698, DateTimeKind.Unspecified).AddTicks(1712),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 6, 8, 18, 24, 28, 718, DateTimeKind.Unspecified).AddTicks(7108),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "EnqueueDate",
                "InternalCommands");

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
                    new DateTime(2021, 7, 10, 10, 21, 8, 698, DateTimeKind.Unspecified).AddTicks(1712),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}