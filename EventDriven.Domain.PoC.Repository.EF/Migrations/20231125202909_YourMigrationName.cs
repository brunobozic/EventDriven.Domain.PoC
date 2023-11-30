using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class YourMigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "OutboxMessages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 25, 20, 29, 8, 922, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 16, 19, 45, 9, 183, DateTimeKind.Unspecified).AddTicks(7428), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "OutboxMessages");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 16, 19, 45, 9, 183, DateTimeKind.Unspecified).AddTicks(7428), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 25, 20, 29, 8, 922, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}