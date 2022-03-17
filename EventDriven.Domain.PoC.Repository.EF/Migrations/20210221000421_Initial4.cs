using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "UserRoles");

            migrationBuilder.DropColumn(
                "IsDraft",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Active",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ActiveTo",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "IsDraft",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "RefreshToken");

            migrationBuilder.DropColumn(
                "CreatedBy",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "DeletedBy",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "IsDraft",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "UserId",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "IsDraft",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "Active",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "ActiveFrom",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "CreatedBy",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "DeletedBy",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "IsDraft",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "LastModifiedBy",
                "AccountJournalEntry");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "AccountJournalEntry");

            migrationBuilder.RenameColumn(
                "LastModifiedBy",
                "UserRoles",
                "UndeletedById");

            migrationBuilder.RenameColumn(
                "LastModifiedBy",
                "RefreshToken",
                "LastModifiedById");

            migrationBuilder.RenameColumn(
                "LastModifiedBy",
                schema: "Audit",
                table: "DbAuditTrail",
                newName: "UndeletedById");

            migrationBuilder.RenameColumn(
                "LastModifiedBy",
                "ApplicationUsers",
                "UndeletedById");

            migrationBuilder.RenameColumn(
                "LastModifiedBy",
                "ApplicationRoles",
                "UndeletedById");

            migrationBuilder.RenameColumn(
                "ActiveTo",
                "AccountJournalEntry",
                "Message");

            migrationBuilder.AddColumn<long>(
                "ActivatedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeactivateReason",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeactivatedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeleteReason",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeletedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ModifiedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ReactivatedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ReactivatedReason",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "UndeleteReason",
                "UserRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
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
                "ActivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeactivateReason",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeleteReason",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeletedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ModifiedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ReactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ReactivatedReason",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "UndeleteReason",
                "ApplicationUsers",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ActivatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeactivateReason",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeactivatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "DeleteReason",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "DeletedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ModifiedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ReactivatedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ReactivatedReason",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "UndeleteReason",
                "ApplicationRoles",
                "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                    "Id",
                    "AccountJournalEntry",
                    "INTEGER",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ActivatedById",
                "UserRoles",
                "ActivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_CreatedById",
                "UserRoles",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_DeactivatedById",
                "UserRoles",
                "DeactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_DeletedById",
                "UserRoles",
                "DeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_LastModifiedById",
                "UserRoles",
                "LastModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ModifiedById",
                "UserRoles",
                "ModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_RectivatedById",
                "UserRoles",
                "ReactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles",
                "UndeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_RefreshToken_LastModifiedById",
                "RefreshToken",
                "LastModifiedById");

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
                "IX_DbAuditTrail_RectivatedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_DbAuditTrail_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers",
                "ActivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers",
                "DeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_LastModifiedById",
                "ApplicationUsers",
                "LastModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_RectivatedById",
                "ApplicationUsers",
                "ReactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers",
                "UndeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_ActivatedById",
                "ApplicationRoles",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_CreatedById",
                "ApplicationRoles",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_DeactivatedById",
                "ApplicationRoles",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_DeletedById",
                "ApplicationRoles",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_LastModifiedById",
                "ApplicationRoles",
                "LastModifiedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_ModifiedById",
                "ApplicationRoles",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_RectivatedById",
                "ApplicationRoles",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_UndeletedById",
                "ApplicationRoles",
                "UndeletedById");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_CreatedById",
                "ApplicationRoles",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                "ApplicationRoles",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                "ApplicationRoles",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_LastModifiedById",
                "ApplicationRoles",
                "LastModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_RectivatedById",
                "ApplicationRoles",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                "ApplicationRoles",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                "ApplicationUsers",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                "ApplicationUsers",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_LastModifiedById",
                "ApplicationUsers",
                "LastModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_RectivatedById",
                "ApplicationUsers",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                "ApplicationUsers",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                "FK_DbAuditTrail_ApplicationUsers_RectivatedById",
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

            migrationBuilder.AddForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken",
                "ApplicationUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_RefreshToken_ApplicationUsers_LastModifiedById",
                "RefreshToken",
                "LastModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ActivatedById",
                "UserRoles",
                "ActivatedById",
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

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_DeactivatedById",
                "UserRoles",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_DeletedById",
                "UserRoles",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_LastModifiedById",
                "UserRoles",
                "LastModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_RectivatedById",
                "UserRoles",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_UndeletedById",
                "UserRoles",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_Id",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_CreatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_RectivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_RectivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

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
                "FK_DbAuditTrail_ApplicationUsers_RectivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_DbAuditTrail_ApplicationUsers_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken");

            migrationBuilder.DropForeignKey(
                "FK_RefreshToken_ApplicationUsers_LastModifiedById",
                "RefreshToken");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ActivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_CreatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeactivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_LastModifiedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_RectivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UndeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_ActivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_CreatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_DeactivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_DeletedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_LastModifiedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_ModifiedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_RectivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_RefreshToken_LastModifiedById",
                "RefreshToken");

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
                "IX_DbAuditTrail_RectivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_DbAuditTrail_UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_CreatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_RectivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_ActivatedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_CreatedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_DeactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_DeletedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_ModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_RectivatedById",
                "ApplicationRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_UndeletedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ActivatedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "CreatedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeactivateReason",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeactivatedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeleteReason",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeletedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ModifiedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ReactivatedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ReactivatedReason",
                "UserRoles");

            migrationBuilder.DropColumn(
                "UndeleteReason",
                "UserRoles");

            migrationBuilder.DropColumn(
                "ActivatedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "CreatedById",
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
                "LastModifiedById",
                schema: "Audit",
                table: "DbAuditTrail");

            migrationBuilder.DropColumn(
                "ModifiedById",
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
                "ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "CreatedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeactivateReason",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeleteReason",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ReactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ReactivatedReason",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "UndeleteReason",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ActivatedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "CreatedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DeactivateReason",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DeactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DeleteReason",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "DeletedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ReactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "ReactivatedReason",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "UndeleteReason",
                "ApplicationRoles");

            migrationBuilder.RenameColumn(
                "UndeletedById",
                "UserRoles",
                "LastModifiedBy");

            migrationBuilder.RenameColumn(
                "LastModifiedById",
                "RefreshToken",
                "LastModifiedBy");

            migrationBuilder.RenameColumn(
                "UndeletedById",
                schema: "Audit",
                table: "DbAuditTrail",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                "UndeletedById",
                "ApplicationUsers",
                "LastModifiedBy");

            migrationBuilder.RenameColumn(
                "UndeletedById",
                "ApplicationRoles",
                "LastModifiedBy");

            migrationBuilder.RenameColumn(
                "Message",
                "AccountJournalEntry",
                "ActiveTo");

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "Active",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveFrom",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ActiveTo",
                "RefreshToken",
                "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "RefreshToken",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                schema: "Audit",
                table: "DbAuditTrail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                "UserId",
                "ApplicationUsers",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "IsDraft",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                    "Id",
                    "AccountJournalEntry",
                    "INTEGER",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

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

            migrationBuilder.AddColumn<long>(
                "CreatedBy",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DeletedBy",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddColumn<long>(
                "ModifiedBy",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken",
                "ApplicationUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}