using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                "StatusId",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 4, 29, 8, 33, 25, 93, DateTimeKind.Unspecified).AddTicks(9708),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 4, 11, 17, 4, 41, 787, DateTimeKind.Unspecified).AddTicks(9815),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "StatusId",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 4, 11, 17, 4, 41, 787, DateTimeKind.Unspecified).AddTicks(9815),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 4, 29, 8, 33, 25, 93, DateTimeKind.Unspecified).AddTicks(9708),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}