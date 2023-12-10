using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class addedstuff15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "PromotedToAdminByUserId",
                "ApplicationUsers");

            migrationBuilder.EnsureSchema(
                "Address");

            migrationBuilder.EnsureSchema(
                "Misc");

            migrationBuilder.AddColumn<Guid>(
                "UserRoleGuid",
                "UserRoles",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                "AddressId",
                "ApplicationUsers",
                "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 12, 21, 8, 43, 840, DateTimeKind.Unspecified).AddTicks(2489),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 6, 21, 54, 45, 332, DateTimeKind.Unspecified).AddTicks(7125),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                "CityBlocks",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityBlocks", x => x.Id);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlocks_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Counties",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.Id);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Counties_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Countries",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Towns",
                schema: "Misc",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedTime = table.Column<string>("TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>("TEXT", nullable: true),
                    LastModifiedTime = table.Column<string>("TEXT", nullable: true),
                    ZipCode = table.Column<string>("TEXT", nullable: true),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Towns_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Types",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Types_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Addresses",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    Line1 = table.Column<string>("TEXT", maxLength: 50, nullable: false),
                    Line2 = table.Column<string>("TEXT", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>("TEXT", maxLength: 50, nullable: false),
                    HouseNumber = table.Column<int>("INTEGER", nullable: false),
                    HouseNumberSuffix = table.Column<string>("TEXT", maxLength: 50, nullable: true),
                    FlatNr = table.Column<int>("INTEGER", nullable: true),
                    UserComment = table.Column<string>("TEXT", maxLength: 250, nullable: true),
                    TownId = table.Column<long>("INTEGER", nullable: true),
                    CountyId = table.Column<long>("INTEGER", nullable: true),
                    CityBlockId = table.Column<long>("INTEGER", nullable: true),
                    CountryId = table.Column<long>("INTEGER", nullable: true),
                    AddressTypeId = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_CityBlocks_CityBlockId",
                        x => x.CityBlockId,
                        principalSchema: "Address",
                        principalTable: "CityBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_Counties_CountyId",
                        x => x.CountyId,
                        principalSchema: "Address",
                        principalTable: "Counties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_Countries_CountryId",
                        x => x.CountryId,
                        principalSchema: "Address",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_Towns_TownId",
                        x => x.TownId,
                        principalSchema: "Misc",
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Addresses_Types_AddressTypeId",
                        x => x.AddressTypeId,
                        principalSchema: "Address",
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "UserAddress",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>("INTEGER", nullable: false),
                    AddressId = table.Column<long>("INTEGER", nullable: false),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    UserRoleGuid = table.Column<Guid>("TEXT", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    Name = table.Column<string>("TEXT", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        "FK_UserAddress_Addresses_AddressId",
                        x => x.AddressId,
                        principalSchema: "Address",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserAddress_ApplicationUsers_UserId",
                        x => x.UserId,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_AddressId",
                "ApplicationUsers",
                "AddressId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_ActivatedById",
                schema: "Address",
                table: "Addresses",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_AddressTypeId",
                schema: "Address",
                table: "Addresses",
                column: "AddressTypeId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CityBlockId",
                schema: "Address",
                table: "Addresses",
                column: "CityBlockId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CountryId",
                schema: "Address",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CountyId",
                schema: "Address",
                table: "Addresses",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CreatedById",
                schema: "Address",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_DeactivatedById",
                schema: "Address",
                table: "Addresses",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_DeletedById",
                schema: "Address",
                table: "Addresses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_ModifiedById",
                schema: "Address",
                table: "Addresses",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_ReactivatedById",
                schema: "Address",
                table: "Addresses",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Addresses_TownId",
                schema: "Address",
                table: "Addresses",
                column: "TownId");

            migrationBuilder.CreateIndex(
                "IX_Addresses_UndeletedById",
                schema: "Address",
                table: "Addresses",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_ActivatedById",
                schema: "Address",
                table: "CityBlocks",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_CreatedById",
                schema: "Address",
                table: "CityBlocks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_DeactivatedById",
                schema: "Address",
                table: "CityBlocks",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_DeletedById",
                schema: "Address",
                table: "CityBlocks",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_ModifiedById",
                schema: "Address",
                table: "CityBlocks",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_ReactivatedById",
                schema: "Address",
                table: "CityBlocks",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlocks_UndeletedById",
                schema: "Address",
                table: "CityBlocks",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_ActivatedById",
                schema: "Address",
                table: "Counties",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_CreatedById",
                schema: "Address",
                table: "Counties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_DeactivatedById",
                schema: "Address",
                table: "Counties",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_DeletedById",
                schema: "Address",
                table: "Counties",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_ModifiedById",
                schema: "Address",
                table: "Counties",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_ReactivatedById",
                schema: "Address",
                table: "Counties",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Counties_UndeletedById",
                schema: "Address",
                table: "Counties",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_ActivatedById",
                schema: "Address",
                table: "Countries",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_CreatedById",
                schema: "Address",
                table: "Countries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_DeactivatedById",
                schema: "Address",
                table: "Countries",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_DeletedById",
                schema: "Address",
                table: "Countries",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_ModifiedById",
                schema: "Address",
                table: "Countries",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_ReactivatedById",
                schema: "Address",
                table: "Countries",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Countries_UndeletedById",
                schema: "Address",
                table: "Countries",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_ActivatedById",
                schema: "Misc",
                table: "Towns",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_CreatedById",
                schema: "Misc",
                table: "Towns",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_DeactivatedById",
                schema: "Misc",
                table: "Towns",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_DeletedById",
                schema: "Misc",
                table: "Towns",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_ModifiedById",
                schema: "Misc",
                table: "Towns",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_ReactivatedById",
                schema: "Misc",
                table: "Towns",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Towns_UndeletedById",
                schema: "Misc",
                table: "Towns",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Types_ActivatedById",
                schema: "Address",
                table: "Types",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Types_CreatedById",
                schema: "Address",
                table: "Types",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Types_DeactivatedById",
                schema: "Address",
                table: "Types",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Types_DeletedById",
                schema: "Address",
                table: "Types",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Types_ModifiedById",
                schema: "Address",
                table: "Types",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Types_ReactivatedById",
                schema: "Address",
                table: "Types",
                column: "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Types_UndeletedById",
                schema: "Address",
                table: "Types",
                column: "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_ActivatedById",
                "UserAddress",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_AddressId",
                "UserAddress",
                "AddressId");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_CreatedById",
                "UserAddress",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_DeactivatedById",
                "UserAddress",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_DeletedById",
                "UserAddress",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_ModifiedById",
                "UserAddress",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_ReactivatedById",
                "UserAddress",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_UndeletedById",
                "UserAddress",
                "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_UserAddress_UserId",
                "UserAddress",
                "UserId");

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_Addresses_AddressId",
                "ApplicationUsers",
                "AddressId",
                principalSchema: "Address",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_Addresses_AddressId",
                "ApplicationUsers");

            migrationBuilder.DropTable(
                "UserAddress");

            migrationBuilder.DropTable(
                "Addresses",
                "Address");

            migrationBuilder.DropTable(
                "CityBlocks",
                "Address");

            migrationBuilder.DropTable(
                "Counties",
                "Address");

            migrationBuilder.DropTable(
                "Countries",
                "Address");

            migrationBuilder.DropTable(
                "Towns",
                "Misc");

            migrationBuilder.DropTable(
                "Types",
                "Address");

            migrationBuilder.DropIndex(
                "IX_ApplicationUsers_AddressId",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "UserRoleGuid",
                "UserRoles");

            migrationBuilder.DropColumn(
                "AddressId",
                "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                "PromotedToAdminByUserId",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 6, 21, 54, 45, 332, DateTimeKind.Unspecified).AddTicks(7125),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 12, 21, 8, 43, 840, DateTimeKind.Unspecified).AddTicks(2489),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}