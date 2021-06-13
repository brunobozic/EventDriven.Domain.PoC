using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeactivatedByUserId",
                "ApplicationUsers");

            migrationBuilder.AddColumn<long>(
                "ActivatedByUserId",
                "UserRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                "DeactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                "ActivatedByUserId",
                "ApplicationUsers",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                "DeactivatedById1",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "ActivatedByUserId",
                "ApplicationRoles",
                "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers",
                "DeactivatedById1",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers",
                "DeactivatedById1",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "UserRoles");

            migrationBuilder.DropColumn(
                "DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "ActivatedByUserId",
                "ApplicationRoles");

            migrationBuilder.AlterColumn<long>(
                "DeactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                "ActivatedByUserId",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                "DeactivatedByUserId",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}