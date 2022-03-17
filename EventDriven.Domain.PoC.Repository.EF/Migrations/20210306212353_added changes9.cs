using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_Id",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropIndex(
                "IX_AccountJournalEntry_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "DateModified",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Description",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Name",
                "AccountJournalEntry");

            migrationBuilder.RenameColumn(
                "UserId",
                "AccountJournalEntry",
                "UserActedUponId");

            migrationBuilder.RenameColumn(
                "Id",
                "AccountJournalEntry",
                "JournalId");

            migrationBuilder.AddColumn<long>(
                "ActingUserId",
                "AccountJournalEntry",
                "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_AccountJournalEntry_ActingUserId",
                "AccountJournalEntry",
                "ActingUserId");

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry",
                "ActingUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry",
                "JournalId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry");

            migrationBuilder.DropIndex(
                "IX_AccountJournalEntry_ActingUserId",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "ActingUserId",
                "AccountJournalEntry");

            migrationBuilder.RenameColumn(
                "UserActedUponId",
                "AccountJournalEntry",
                "UserId");

            migrationBuilder.RenameColumn(
                "JournalId",
                "AccountJournalEntry",
                "Id");

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateModified",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Description",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_AccountJournalEntry_UserId",
                "AccountJournalEntry",
                "UserId");

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_Id",
                "AccountJournalEntry",
                "Id",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}