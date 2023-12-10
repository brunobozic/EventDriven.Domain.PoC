using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_LastModifiedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_ActivatedById",
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
                "IX_UserRoles_ReactivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationRoles_LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "UserRoles");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "LastModifiedById",
                "ApplicationRoles");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ActivatedById",
                "UserRoles",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_DeactivatedById",
                "UserRoles",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_DeletedById",
                "UserRoles",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ModifiedById",
                "UserRoles",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ReactivatedById",
                "UserRoles",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles",
                "UndeletedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_UserRoles_ActivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_DeactivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_DeletedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_ModifiedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_ReactivatedById",
                "UserRoles");

            migrationBuilder.DropIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles");

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "LastModifiedById",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_ActivatedById",
                "UserRoles",
                "ActivatedById",
                unique: true);

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
                "IX_UserRoles_ReactivatedById",
                "UserRoles",
                "ReactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_UserRoles_UndeletedById",
                "UserRoles",
                "UndeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_LastModifiedById",
                "ApplicationUsers",
                "LastModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationRoles_LastModifiedById",
                "ApplicationRoles",
                "LastModifiedById");

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_LastModifiedById",
                "ApplicationRoles",
                "LastModifiedById",
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
                "FK_UserRoles_ApplicationUsers_LastModifiedById",
                "UserRoles",
                "LastModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}