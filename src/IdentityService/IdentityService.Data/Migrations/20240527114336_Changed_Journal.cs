using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace IdentityService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Journal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                table: "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                table: "AccountJournalEntry");

            migrationBuilder.DropIndex(
                name: "IX_AccountJournalEntry_ActingUserId",
                table: "AccountJournalEntry");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 5, 27, 11, 43, 35, 917, DateTimeKind.Unspecified).AddTicks(6620), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 5, 27, 10, 52, 36, 149, DateTimeKind.Unspecified).AddTicks(5083), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountJournalEntry_UserId",
                table: "AccountJournalEntry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_UserId",
                table: "AccountJournalEntry",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_UserId",
                table: "AccountJournalEntry");

            migrationBuilder.DropIndex(
                name: "IX_AccountJournalEntry_UserId",
                table: "AccountJournalEntry");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AccountJournalEntry");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 5, 27, 10, 52, 36, 149, DateTimeKind.Unspecified).AddTicks(5083), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 5, 27, 11, 43, 35, 917, DateTimeKind.Unspecified).AddTicks(6620), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_AccountJournalEntry_ActingUserId",
                table: "AccountJournalEntry",
                column: "ActingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                table: "AccountJournalEntry",
                column: "ActingUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                table: "AccountJournalEntry",
                column: "JournalId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
