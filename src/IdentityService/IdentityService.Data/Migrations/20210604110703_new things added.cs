using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class newthingsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Status",
                "ApplicationUsers");

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
                    new DateTime(2021, 4, 29, 8, 33, 25, 93, DateTimeKind.Unspecified).AddTicks(9708),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                "Status",
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
                    new DateTime(2021, 6, 4, 11, 7, 2, 731, DateTimeKind.Unspecified).AddTicks(1190),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}