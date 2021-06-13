using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedchanges7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ReactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "DeactivatedById1",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<long>(
                "DeactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                "UserId",
                "AccountJournalEntry",
                "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ReactivatedById",
                "ApplicationUsers",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers",
                "UndeletedById");

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry",
                "UserId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_ReactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                "DeactivatedById",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                "DeactivatedById1",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                "UserId",
                "AccountJournalEntry",
                "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ActivatedById",
                "ApplicationUsers",
                "ActivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers",
                "DeactivatedById1",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_DeletedById",
                "ApplicationUsers",
                "DeletedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_ReactivatedById",
                "ApplicationUsers",
                "ReactivatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_UndeletedById",
                "ApplicationUsers",
                "UndeletedById",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_UserId",
                "AccountJournalEntry",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById1",
                "ApplicationUsers",
                "DeactivatedById1",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}