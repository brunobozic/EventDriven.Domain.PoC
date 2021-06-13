using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "Audit");

            migrationBuilder.CreateTable(
                "ApplicationRoles",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    CreatedBy = table.Column<long>("INTEGER", nullable: false),
                    ModifiedBy = table.Column<long>("INTEGER", nullable: false),
                    DeletedBy = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ApplicationRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "ApplicationUsers",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>("TEXT", nullable: true),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    Oib = table.Column<string>("TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    FirstName = table.Column<string>("TEXT", nullable: true),
                    LastName = table.Column<string>("TEXT", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreateddBy = table.Column<long>("INTEGER", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    LastModifiedBy = table.Column<long>("INTEGER", nullable: true),
                    Email = table.Column<string>("TEXT", nullable: true),
                    UserName = table.Column<string>("TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>("TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>("TEXT", nullable: true),
                    TwoFactorEnabled = table.Column<bool>("INTEGER", nullable: false),
                    Verified = table.Column<DateTime>("TEXT", nullable: false),
                    ActivatedByUserId = table.Column<int>("INTEGER", nullable: false),
                    ResetToken = table.Column<string>("TEXT", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>("TEXT", nullable: true),
                    PasswordHash = table.Column<string>("TEXT", nullable: true),
                    PasswordReset = table.Column<DateTime>("TEXT", nullable: true),
                    VerificationToken = table.Column<string>("TEXT", nullable: true),
                    VerificationTokenExpirationDate = table.Column<DateTime>("TEXT", nullable: true),
                    Status = table.Column<byte>("INTEGER", nullable: false),
                    Created = table.Column<DateTime>("TEXT", nullable: false),
                    Updated = table.Column<DateTime>("TEXT", nullable: false),
                    PasswordResetMsg = table.Column<string>("TEXT", nullable: true),
                    PromotedToAdminByUserId = table.Column<int>("INTEGER", nullable: false),
                    UserId = table.Column<Guid>("TEXT", nullable: false),
                    VerificationFailureLatestMessage = table.Column<string>("TEXT", nullable: true),
                    LastVerificationFailureDate = table.Column<DateTime>("TEXT", nullable: false),
                    DectivatedByUserId = table.Column<int>("INTEGER", nullable: false),
                    CreatedBy = table.Column<long>("INTEGER", nullable: false),
                    ModifiedBy = table.Column<long>("INTEGER", nullable: false),
                    DeletedBy = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ApplicationUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "DbAuditTrail",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TableName = table.Column<string>("TEXT", nullable: true),
                    UserId = table.Column<int>("INTEGER", nullable: false),
                    UserName = table.Column<string>("TEXT", nullable: true),
                    OldData = table.Column<string>("TEXT", nullable: true),
                    NewData = table.Column<string>("TEXT", nullable: true),
                    TableIdValue = table.Column<long>("INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    Actions = table.Column<string>("TEXT", nullable: true),
                    CreatedBy = table.Column<long>("INTEGER", nullable: false),
                    ModifiedBy = table.Column<long>("INTEGER", nullable: false),
                    DeletedBy = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_DbAuditTrail", x => x.Id); });

            migrationBuilder.CreateTable(
                "InternalCommands",
                table => new
                {
                    Id = table.Column<Guid>("TEXT", nullable: false),
                    Type = table.Column<string>("TEXT", nullable: true),
                    Data = table.Column<string>("TEXT", nullable: true),
                    ProcessedDate = table.Column<DateTime>("TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_InternalCommands", x => x.Id); });

            migrationBuilder.CreateTable(
                "OutboxMessages",
                table => new
                {
                    Id = table.Column<Guid>("TEXT", nullable: false),
                    OccurredOn = table.Column<DateTime>("TEXT", nullable: false),
                    Type = table.Column<string>("TEXT", nullable: true),
                    Data = table.Column<string>("TEXT", nullable: true),
                    ProcessedDate = table.Column<DateTime>("TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_OutboxMessages", x => x.Id); });

            migrationBuilder.CreateTable(
                "AccountJournalEntry",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>("INTEGER", nullable: false),
                    CreatedBy = table.Column<long>("INTEGER", nullable: false),
                    ModifiedBy = table.Column<long>("INTEGER", nullable: false),
                    DeletedBy = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountJournalEntry", x => x.Id);
                    table.ForeignKey(
                        "FK_AccountJournalEntry_ApplicationUsers_UserId",
                        x => x.UserId,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "RefreshToken",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<long>("INTEGER", nullable: false),
                    Token = table.Column<string>("TEXT", nullable: true),
                    Expires = table.Column<DateTime>("TEXT", nullable: false),
                    Created = table.Column<DateTime>("TEXT", nullable: false),
                    CreatedByIp = table.Column<string>("TEXT", nullable: true),
                    Revoked = table.Column<DateTime>("TEXT", nullable: true),
                    RevokedByIp = table.Column<string>("TEXT", nullable: true),
                    ReplacedByToken = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                        x => x.ApplicationUserId,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "UserRoles",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    UserId = table.Column<long>("INTEGER", nullable: false),
                    RoleId = table.Column<long>("INTEGER", nullable: false),
                    CreatedBy = table.Column<long>("INTEGER", nullable: false),
                    ModifiedBy = table.Column<long>("INTEGER", nullable: false),
                    DeletedBy = table.Column<long>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        "FK_UserRoles_ApplicationRoles_RoleId",
                        x => x.RoleId,
                        "ApplicationRoles",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserRoles_ApplicationUsers_UserId",
                        x => x.UserId,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AccountJournalEntry_UserId",
                "AccountJournalEntry",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_RefreshToken_ApplicationUserId",
                "RefreshToken",
                "ApplicationUserId");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_RoleId",
                "UserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_UserRoles_UserId",
                "UserRoles",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AccountJournalEntry");

            migrationBuilder.DropTable(
                "DbAuditTrail",
                "Audit");

            migrationBuilder.DropTable(
                "InternalCommands");

            migrationBuilder.DropTable(
                "OutboxMessages");

            migrationBuilder.DropTable(
                "RefreshToken");

            migrationBuilder.DropTable(
                "UserRoles");

            migrationBuilder.DropTable(
                "ApplicationRoles");

            migrationBuilder.DropTable(
                "ApplicationUsers");
        }
    }
}