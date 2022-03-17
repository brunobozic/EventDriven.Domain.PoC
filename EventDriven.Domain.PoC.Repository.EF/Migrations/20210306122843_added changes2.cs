using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_CreatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_DeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropPrimaryKey(
                "PK_DbAuditTrail",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_CreatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_DeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_ModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Id",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

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
                "CreatedById",
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
                "DeactivateReason",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DeleteReason",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Description",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "Name",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ReactivatedReason",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "UndeleteReason",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.RenameColumn(
                "TheUserHasBeenDeleted",
                schema: "Audit",
                table: "DbAuditTrail",
                newName: "AuditTrailId");

            migrationBuilder.AlterColumn<long>(
                    "AuditTrailId",
                    schema: "Audit",
                    table: "DbAuditTrail",
                    type: "INTEGER",
                    nullable: false,
                    oldClrType: typeof(bool),
                    oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                "PK_DbAuditTrail",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "AuditTrailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_DbAuditTrail",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.RenameColumn(
                "AuditTrailId",
                schema: "Audit",
                table: "DbAuditTrail",
                newName: "TheUserHasBeenDeleted");

            migrationBuilder.AlterColumn<bool>(
                    "TheUserHasBeenDeleted",
                    schema: "Audit",
                    table: "DbAuditTrail",
                    type: "INTEGER",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                    "Id",
                    schema: "Audit",
                    table: "DbAuditTrail",
                    type: "INTEGER",
                    nullable: false,
                    defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                "ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
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

            migrationBuilder.AddColumn<long>(
                "CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddColumn<string>(
                "DeactivateReason",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeleteReason",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Description",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ModifiedById",
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

            migrationBuilder.AddColumn<long>(
                "ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ReactivatedReason",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "UndeleteReason",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_DbAuditTrail",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "Id");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_DeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_ModifiedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "UndeletedById");

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ActivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_DeactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "DeactivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_DeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "DeletedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "LastModifiedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ModifiedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ModifiedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_ReactivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ReactivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "UndeletedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}