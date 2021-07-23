using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_CreatedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "IsActive",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DateCreated",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "TheUserHasBeenDeleted",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "CreateddBy",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DateCreated",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "TheUserHasBeenDeleted",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Message",
                "AccountJournalEntry");

            migrationBuilder.RenameColumn(
                "DectivatedByUserId",
                "ApplicationUsers",
                "DeactivatedByUserId");

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "ActiveFrom",
                "UserRoles",
                "TEXT",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_CreatedById",
                "UserRoles",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_CreatedById",
                "UserRoles");

            migrationBuilder.RenameColumn(
                "DeactivatedByUserId",
                "ApplicationUsers",
                "DectivatedByUserId");

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "UserRoles",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "ActiveFrom",
                "UserRoles",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsActive",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "RefreshToken",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                "TheUserHasBeenDeleted",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                "CreateddBy",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                "CreatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                "TheUserHasBeenDeleted",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "Message",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_CreatedById",
                "UserRoles",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}