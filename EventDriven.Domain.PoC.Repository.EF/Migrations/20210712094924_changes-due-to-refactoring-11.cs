using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changesduetorefactoring11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "UserName",
                "ApplicationUsers",
                "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Oib",
                "ApplicationUsers",
                "TEXT",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Email",
                "ApplicationUsers",
                "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 49, 24, 460, DateTimeKind.Unspecified).AddTicks(6445),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_Email",
                "ApplicationUsers",
                "Email");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_Oib",
                "ApplicationUsers",
                "Oib",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_UserName",
                "ApplicationUsers",
                "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_Email",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_Oib",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_UserName",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<string>(
                "UserName",
                "ApplicationUsers",
                "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                "Oib",
                "ApplicationUsers",
                "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                "Email",
                "ApplicationUsers",
                "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 23, 54, 67, DateTimeKind.Unspecified).AddTicks(1226),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 7, 12, 9, 49, 24, 460, DateTimeKind.Unspecified).AddTicks(6445),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}