using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class stuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CityBlock_CityBlockId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Country_CountryId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_County_CountyId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Town_TownId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_ApplicationUsers_LastModifiedById",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "CityBlock");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "County");

            migrationBuilder.DropTable(
                name: "Town");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_LastModifiedById",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CityBlockId1",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryId1",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountyId1",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_TownId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "PasswordResetMsg",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "UserResourceId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CityBlockId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountyId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "TownId1",
                table: "Addresses");

            migrationBuilder.EnsureSchema(
                name: "Codebook");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 4, 24, 9, 42, 11, 735, DateTimeKind.Unspecified).AddTicks(5348), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 25, 20, 29, 8, 922, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)));

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
                name: "IX_Countries_ActivatedById",
                schema: "Codebook",
                table: "Countries",
                column: "ActivatedById");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Codebook");

            migrationBuilder.DropTable(
                name: "InboxMessages");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Codebook");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "RefreshToken",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateModified",
                table: "RefreshToken",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "RefreshToken",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetMsg",
                table: "ApplicationUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserResourceId",
                table: "ApplicationUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "AddressTypeId",
                table: "Addresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CityBlockId1",
                table: "Addresses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CountryId1",
                table: "Addresses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CountyId1",
                table: "Addresses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownId1",
                table: "Addresses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "AccountJournalEntry",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 25, 20, 29, 8, 922, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 4, 24, 9, 42, 11, 735, DateTimeKind.Unspecified).AddTicks(5348), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "CityBlock",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityBlock_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Country_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "County",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_County", x => x.Id);
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_County_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Town",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReactivatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UndeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeactivateReason = table.Column<string>(type: "TEXT", nullable: true),
                    DeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>(type: "TEXT", nullable: true),
                    UndeleteReason = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Town", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_ReactivatedById",
                        column: x => x.ReactivatedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Town_ApplicationUsers_UndeletedById",
                        column: x => x.UndeletedById,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_LastModifiedById",
                table: "RefreshToken",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityBlockId1",
                table: "Addresses",
                column: "CityBlockId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId1",
                table: "Addresses",
                column: "CountryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountyId1",
                table: "Addresses",
                column: "CountyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TownId1",
                table: "Addresses",
                column: "TownId1");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_ActivatedById",
                table: "CityBlock",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_CreatedById",
                table: "CityBlock",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_DeactivatedById",
                table: "CityBlock",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_DeletedById",
                table: "CityBlock",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_ModifiedById",
                table: "CityBlock",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_ReactivatedById",
                table: "CityBlock",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CityBlock_UndeletedById",
                table: "CityBlock",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_ActivatedById",
                table: "Country",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CreatedById",
                table: "Country",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_DeactivatedById",
                table: "Country",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_DeletedById",
                table: "Country",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_ModifiedById",
                table: "Country",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_ReactivatedById",
                table: "Country",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_UndeletedById",
                table: "Country",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_ActivatedById",
                table: "County",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_CreatedById",
                table: "County",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_DeactivatedById",
                table: "County",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_DeletedById",
                table: "County",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_ModifiedById",
                table: "County",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_ReactivatedById",
                table: "County",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_County_UndeletedById",
                table: "County",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_ActivatedById",
                table: "Town",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_CreatedById",
                table: "Town",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_DeactivatedById",
                table: "Town",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_DeletedById",
                table: "Town",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_ModifiedById",
                table: "Town",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_ReactivatedById",
                table: "Town",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Town_UndeletedById",
                table: "Town",
                column: "UndeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CityBlock_CityBlockId1",
                table: "Addresses",
                column: "CityBlockId1",
                principalTable: "CityBlock",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Country_CountryId1",
                table: "Addresses",
                column: "CountryId1",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_County_CountyId1",
                table: "Addresses",
                column: "CountyId1",
                principalTable: "County",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Town_TownId1",
                table: "Addresses",
                column: "TownId1",
                principalTable: "Town",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_ApplicationUsers_LastModifiedById",
                table: "RefreshToken",
                column: "LastModifiedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }
    }
}
