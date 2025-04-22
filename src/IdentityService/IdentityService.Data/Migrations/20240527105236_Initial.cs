using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace IdentityService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Codebook");

            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StatusId = table.Column<byte>(type: "INTEGER", nullable: false),
                    DateOfBirth = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    LastVerificationFailureDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LatestVerificationFailureMessage = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Oib = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordReset = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ResetToken = table.Column<string>(type: "TEXT", nullable: true),
                    ResetTokenExpires = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VerificationTokenExpirationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Verified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DbAuditTrail",
                schema: "Audit",
                columns: table => new
                {
                    AuditTrailId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Actions = table.Column<string>(type: "TEXT", nullable: true),
                    NewData = table.Column<string>(type: "TEXT", nullable: true),
                    OldData = table.Column<string>(type: "TEXT", nullable: true),
                    TableIdValue = table.Column<long>(type: "INTEGER", nullable: true),
                    TableIdValueGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbAuditTrail", x => x.AuditTrailId);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalCommands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    EnqueueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    OccurredOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ActivatedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountJournalEntry",
                columns: table => new
                {
                    JournalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActingEmail = table.Column<string>(type: "TEXT", nullable: true),
                    ActingUserName = table.Column<string>(type: "TEXT", nullable: true),
                    EmailActedUpon = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Seen = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UserNameActedUpon = table.Column<string>(type: "TEXT", nullable: false),
                    ActingUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserActedUponId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 5, 27, 10, 52, 36, 149, DateTimeKind.Unspecified).AddTicks(5083), new TimeSpan(0, 0, 0, 0, 0))),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountJournalEntry", x => x.JournalId);
                    table.ForeignKey(
                        name: "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                        column: x => x.ActingUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                        column: x => x.JournalId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Line1 = table.Column<string>(type: "TEXT", nullable: false),
                    Line2 = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    HouseNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    HouseNumberSuffix = table.Column<string>(type: "TEXT", nullable: true),
                    FlatNr = table.Column<int>(type: "INTEGER", nullable: true),
                    UserComment = table.Column<string>(type: "TEXT", nullable: true),
                    AddressIdGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    AddressTypeId = table.Column<int>(name: "AddressTypeId                                   ", type: "INTEGER", nullable: true),
                    CityBlockId = table.Column<int>(type: "INTEGER", nullable: true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: true),
                    CountyId = table.Column<int>(type: "INTEGER", nullable: true),
                    TownId = table.Column<int>(type: "INTEGER", nullable: true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AddressTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AddressTypes_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleIdGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Codebook",
                columns: table => new
                {
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    INT_NAME = table.Column<string>(type: "TEXT", maxLength: 75, nullable: false),
                    HR_NAME = table.Column<string>(type: "TEXT", maxLength: 75, nullable: false),
                    ISO_3166_ALPHA_2 = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    ISO_3166_ALPHA_3 = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ISO_3166_NUMERIC = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedByIp = table.Column<string>(type: "TEXT", nullable: true),
                    RevokedByIp = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Expires = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Revoked = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ControllerName = table.Column<string>(type: "TEXT", nullable: true),
                    ControllerActionName = table.Column<string>(type: "TEXT", nullable: true),
                    ReportName = table.Column<string>(type: "TEXT", nullable: true),
                    ResourceType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivatedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedByApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_ApplicationUsers_ActivatedByApplicationUserId",
                        column: x => x.ActivatedByApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_ApplicationUsers_CreatedByApplicationUserId",
                        column: x => x.CreatedByApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_ApplicationUsers_DeletedByApplicationUserId",
                        column: x => x.DeletedByApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_ApplicationUsers_ModifiedByApplicationUserId",
                        column: x => x.ModifiedByApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "Codebook",
                columns: table => new
                {
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenantName = table.Column<string>(type: "TEXT", maxLength: 75, nullable: false),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserRoleGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddressId = table.Column<long>(type: "INTEGER", nullable: false),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddress_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserAddress_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserRoleGuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<long>(type: "INTEGER", nullable: false),
                    ResourceId = table.Column<long>(type: "INTEGER", nullable: false),
                    TestData = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_ApplicationRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountJournalEntry_ActingUserId",
                table: "AccountJournalEntry",
                column: "ActingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ActivatedById",
                table: "Addresses",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedById",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DeactivatedById",
                table: "Addresses",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DeletedById",
                table: "Addresses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ModifiedById",
                table: "Addresses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ReactivatedById",
                table: "Addresses",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UndeletedById",
                table: "Addresses",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_ActivatedById",
                table: "AddressTypes",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_CreatedById",
                table: "AddressTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_DeactivatedById",
                table: "AddressTypes",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_DeletedById",
                table: "AddressTypes",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_ModifiedById",
                table: "AddressTypes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_ReactivatedById",
                table: "AddressTypes",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AddressTypes_UndeletedById",
                table: "AddressTypes",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_ActivatedById",
                table: "ApplicationRoles",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_CreatedById",
                table: "ApplicationRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_DeactivatedById",
                table: "ApplicationRoles",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_DeletedById",
                table: "ApplicationRoles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_ModifiedById",
                table: "ApplicationRoles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_ReactivatedById",
                table: "ApplicationRoles",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoles_UndeletedById",
                table: "ApplicationRoles",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ActivatedById",
                table: "ApplicationUsers",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CreatedById",
                table: "ApplicationUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_DeactivatedById",
                table: "ApplicationUsers",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_DeletedById",
                table: "ApplicationUsers",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Email",
                table: "ApplicationUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ModifiedById",
                table: "ApplicationUsers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Oib",
                table: "ApplicationUsers",
                column: "Oib",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ReactivatedById",
                table: "ApplicationUsers",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_UndeletedById",
                table: "ApplicationUsers",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_UserName",
                table: "ApplicationUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ActivatedById",
                schema: "Codebook",
                table: "Countries",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ApplicationUserId",
                table: "RefreshToken",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ActivatedByApplicationUserId",
                table: "Resources",
                column: "ActivatedByApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CreatedByApplicationUserId",
                table: "Resources",
                column: "CreatedByApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_DeletedByApplicationUserId",
                table: "Resources",
                column: "DeletedByApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ModifiedByApplicationUserId",
                table: "Resources",
                column: "ModifiedByApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_DeletedById",
                table: "RolePermissions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ResourceId",
                table: "RolePermissions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ActivatedById",
                schema: "Codebook",
                table: "Tenants",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_ActivatedById",
                table: "UserAddress",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_AddressId",
                table: "UserAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_CreatedById",
                table: "UserAddress",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_DeactivatedById",
                table: "UserAddress",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_DeletedById",
                table: "UserAddress",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_ModifiedById",
                table: "UserAddress",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_ReactivatedById",
                table: "UserAddress",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UndeletedById",
                table: "UserAddress",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId",
                table: "UserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ActivatedById",
                table: "UserRoles",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedById",
                table: "UserRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DeactivatedById",
                table: "UserRoles",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DeletedById",
                table: "UserRoles",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ModifiedById",
                table: "UserRoles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ReactivatedById",
                table: "UserRoles",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UndeletedById",
                table: "UserRoles",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountJournalEntry");

            migrationBuilder.DropTable(
                name: "AddressTypes");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Codebook");

            migrationBuilder.DropTable(
                name: "DbAuditTrail",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "InboxMessages");

            migrationBuilder.DropTable(
                name: "InternalCommands");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Codebook");

            migrationBuilder.DropTable(
                name: "UserAddress");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "ApplicationRoles");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");
        }
    }
}
