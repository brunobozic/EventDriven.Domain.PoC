using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "Active",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "Description",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "LastModifiedBy",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Active",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveFrom",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveTo",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateDeleted",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateModified",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "Description",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "LastModifiedBy",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Description",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Active",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveFrom",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveTo",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreateddBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "ApplicationRoles",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateDeleted",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateModified",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "Description",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "LastModifiedBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "RoleId",
                "ApplicationRoles",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                "Active",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveFrom",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveTo",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateDeleted",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "DateModified",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Deleted",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "Description",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "LastModifiedBy",
                "AccountJournalEntry",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Name",
                "AccountJournalEntry",
                "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Active",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Deleted",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Description",
                "UserRoles");

            migrationBuilder.DropColumn(
                "IsDraft",
                "UserRoles");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Name",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Active",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ActiveTo",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DateCreated",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DateDeleted",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DateModified",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Deleted",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Description",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "IsDraft",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Name",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Description",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "Name",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "Active",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ActiveTo",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "CreateddBy",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DateCreated",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DateDeleted",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DateModified",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Deleted",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Description",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "IsDraft",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "RoleId",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Active",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "ActiveTo",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "DateCreated",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "DateDeleted",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "DateModified",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Deleted",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Description",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "IsDraft",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "Name",
                "AccountJournalEntry");
        }
    }
}