using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ActivatedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_DeactivatedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_DeletedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ModifiedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ReactivatedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_UndeletedById",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_CityBlocks_CityBlockId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Counties_CountyId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Countries_CountryId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Towns_TownId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Types_AddressTypeId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ReactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_Addresses_AddressId",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ReactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_Addresses_AddressId",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ActivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_CreatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_DeactivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_DeletedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ModifiedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ReactivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_UndeletedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_UserId",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationRoles_RoleId",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ActivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeactivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ReactivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UndeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles");

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

            migrationBuilder.DropIndex(
                "IX_Addresses_AddressTypeId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_CityBlockId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_CountryId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_CountyId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_TownId",
                schema: "Address",
                table: "Addresses");

            migrationBuilder.DropColumn(
                "AddressId",
                "ApplicationUsers");

            migrationBuilder.RenameTable(
                "Addresses",
                "Address",
                "Addresses");

            migrationBuilder.RenameColumn(
                "IsActive",
                "Addresses",
                "Id2");

            migrationBuilder.AddColumn<int>(
                "Id2",
                "UserRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "UserAddress",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "ApplicationUsers",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "Id2",
                "ApplicationRoles",
                "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "DateCreated",
                "AccountJournalEntry",
                "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(2021, 4, 11, 17, 4, 41, 787, DateTimeKind.Unspecified).AddTicks(9815),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(
                    new DateTime(2021, 3, 12, 21, 8, 43, 840, DateTimeKind.Unspecified).AddTicks(2489),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                "PostalCode",
                "Addresses",
                "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<long>(
                "AddressTypeId",
                "Addresses",
                "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                "AddressIdGuid",
                "Addresses",
                "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                "AddressTypeId                                   ",
                "Addresses",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CityBlockId1",
                "Addresses",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CountryId1",
                "Addresses",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "CountyId1",
                "Addresses",
                "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                "TownId1",
                "Addresses",
                "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                "AddressTypes",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id2 = table.Column<int>("INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        "FK_AddressTypes_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                "CityBlock",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id2 = table.Column<int>("INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_CityBlock", x => x.Id);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_CityBlock_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Country",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id2 = table.Column<int>("INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Country_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "County",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id2 = table.Column<int>("INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_County", x => x.Id);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_County_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Town",
                table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id2 = table.Column<int>("INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_Town", x => x.Id);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_ActivatedById",
                        x => x.ActivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_CreatedById",
                        x => x.CreatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_DeactivatedById",
                        x => x.DeactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_DeletedById",
                        x => x.DeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_ModifiedById",
                        x => x.ModifiedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_ReactivatedById",
                        x => x.ReactivatedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Town_ApplicationUsers_UndeletedById",
                        x => x.UndeletedById,
                        "ApplicationUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Addresses_CityBlockId1",
                "Addresses",
                "CityBlockId1");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CountryId1",
                "Addresses",
                "CountryId1");

            migrationBuilder.CreateIndex(
                "IX_Addresses_CountyId1",
                "Addresses",
                "CountyId1");

            migrationBuilder.CreateIndex(
                "IX_Addresses_TownId1",
                "Addresses",
                "TownId1");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_ActivatedById",
                "AddressTypes",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_CreatedById",
                "AddressTypes",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_DeactivatedById",
                "AddressTypes",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_DeletedById",
                "AddressTypes",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_ModifiedById",
                "AddressTypes",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_ReactivatedById",
                "AddressTypes",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_AddressTypes_UndeletedById",
                "AddressTypes",
                "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_ActivatedById",
                "CityBlock",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_CreatedById",
                "CityBlock",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_DeactivatedById",
                "CityBlock",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_DeletedById",
                "CityBlock",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_ModifiedById",
                "CityBlock",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_ReactivatedById",
                "CityBlock",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_CityBlock_UndeletedById",
                "CityBlock",
                "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Country_ActivatedById",
                "Country",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Country_CreatedById",
                "Country",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Country_DeactivatedById",
                "Country",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Country_DeletedById",
                "Country",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Country_ModifiedById",
                "Country",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Country_ReactivatedById",
                "Country",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Country_UndeletedById",
                "Country",
                "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_County_ActivatedById",
                "County",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_County_CreatedById",
                "County",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_County_DeactivatedById",
                "County",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_County_DeletedById",
                "County",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_County_ModifiedById",
                "County",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_County_ReactivatedById",
                "County",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_County_UndeletedById",
                "County",
                "UndeletedById");

            migrationBuilder.CreateIndex(
                "IX_Town_ActivatedById",
                "Town",
                "ActivatedById");

            migrationBuilder.CreateIndex(
                "IX_Town_CreatedById",
                "Town",
                "CreatedById");

            migrationBuilder.CreateIndex(
                "IX_Town_DeactivatedById",
                "Town",
                "DeactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Town_DeletedById",
                "Town",
                "DeletedById");

            migrationBuilder.CreateIndex(
                "IX_Town_ModifiedById",
                "Town",
                "ModifiedById");

            migrationBuilder.CreateIndex(
                "IX_Town_ReactivatedById",
                "Town",
                "ReactivatedById");

            migrationBuilder.CreateIndex(
                "IX_Town_UndeletedById",
                "Town",
                "UndeletedById");

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry",
                "ActingUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry",
                "JournalId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ActivatedById",
                "Addresses",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_DeactivatedById",
                "Addresses",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_DeletedById",
                "Addresses",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ModifiedById",
                "Addresses",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ReactivatedById",
                "Addresses",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_UndeletedById",
                "Addresses",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_CityBlock_CityBlockId1",
                "Addresses",
                "CityBlockId1",
                "CityBlock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Country_CountryId1",
                "Addresses",
                "CountryId1",
                "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_County_CountyId1",
                "Addresses",
                "CountyId1",
                "County",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Town_TownId1",
                "Addresses",
                "TownId1",
                "Town",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                "ApplicationRoles",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                "ApplicationRoles",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ReactivatedById",
                "ApplicationRoles",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                "ApplicationRoles",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                "ApplicationUsers",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                "ApplicationUsers",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ReactivatedById",
                "ApplicationUsers",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                "ApplicationUsers",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken",
                "ApplicationUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_Addresses_AddressId",
                "UserAddress",
                "AddressId",
                "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ActivatedById",
                "UserAddress",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_CreatedById",
                "UserAddress",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_DeactivatedById",
                "UserAddress",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_DeletedById",
                "UserAddress",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ModifiedById",
                "UserAddress",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ReactivatedById",
                "UserAddress",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_UndeletedById",
                "UserAddress",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_UserId",
                "UserAddress",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationRoles_RoleId",
                "UserRoles",
                "RoleId",
                "ApplicationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ActivatedById",
                "UserRoles",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_DeactivatedById",
                "UserRoles",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_DeletedById",
                "UserRoles",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ReactivatedById",
                "UserRoles",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_UndeletedById",
                "UserRoles",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ActivatedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_DeactivatedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_DeletedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ModifiedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_ReactivatedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_ApplicationUsers_UndeletedById",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_CityBlock_CityBlockId1",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Country_CountryId1",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_County_CountyId1",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_Addresses_Town_TownId1",
                "Addresses");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_DeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ReactivatedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_UndeletedById",
                "ApplicationRoles");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ActivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_DeletedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ReactivatedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_UndeletedById",
                "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_Addresses_AddressId",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ActivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_CreatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_DeactivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_DeletedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ModifiedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_ReactivatedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_UndeletedById",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserAddress_ApplicationUsers_UserId",
                "UserAddress");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationRoles_RoleId",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ActivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeactivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_DeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_ReactivatedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UndeletedById",
                "UserRoles");

            migrationBuilder.DropForeignKey(
                "FK_UserRoles_ApplicationUsers_UserId",
                "UserRoles");

            migrationBuilder.DropTable(
                "AddressTypes");

            migrationBuilder.DropTable(
                "CityBlock");

            migrationBuilder.DropTable(
                "Country");

            migrationBuilder.DropTable(
                "County");

            migrationBuilder.DropTable(
                "Town");

            migrationBuilder.DropIndex(
                "IX_Addresses_CityBlockId1",
                "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_CountryId1",
                "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_CountyId1",
                "Addresses");

            migrationBuilder.DropIndex(
                "IX_Addresses_TownId1",
                "Addresses");

            migrationBuilder.DropColumn(
                "Id2",
                "UserRoles");

            migrationBuilder.DropColumn(
                "Id2",
                "UserAddress");

            migrationBuilder.DropColumn(
                "Id2",
                "ApplicationUsers");

            migrationBuilder.DropColumn(
                "Id2",
                "ApplicationRoles");

            migrationBuilder.DropColumn(
                "AddressIdGuid",
                "Addresses");

            migrationBuilder.DropColumn(
                "AddressTypeId                                   ",
                "Addresses");

            migrationBuilder.DropColumn(
                "CityBlockId1",
                "Addresses");

            migrationBuilder.DropColumn(
                "CountryId1",
                "Addresses");

            migrationBuilder.DropColumn(
                "CountyId1",
                "Addresses");

            migrationBuilder.DropColumn(
                "TownId1",
                "Addresses");

            migrationBuilder.EnsureSchema(
                "Address");

            migrationBuilder.EnsureSchema(
                "Misc");

            migrationBuilder.RenameTable(
                "Addresses",
                newName: "Addresses",
                newSchema: "Address");

            migrationBuilder.RenameColumn(
                "Id2",
                schema: "Address",
                table: "Addresses",
                newName: "IsActive");

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
                    new DateTime(2021, 4, 11, 17, 4, 41, 787, DateTimeKind.Unspecified).AddTicks(9815),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                "PostalCode",
                schema: "Address",
                table: "Addresses",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                "AddressTypeId",
                schema: "Address",
                table: "Addresses",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                "CityBlocks",
                schema: "Address",
                columns: table => new
                {
                    Id = table.Column<long>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true)
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
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true)
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
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true)
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
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    CreatedTime = table.Column<string>("TEXT", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>("TEXT", nullable: true),
                    LastModifiedTime = table.Column<string>("TEXT", nullable: true),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true),
                    ZipCode = table.Column<string>("TEXT", nullable: true)
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
                    ActivatedById = table.Column<long>("INTEGER", nullable: true),
                    ActivatedByUserId = table.Column<long>("INTEGER", nullable: true),
                    Active = table.Column<bool>("INTEGER", nullable: false),
                    ActiveFrom = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    CreatedById = table.Column<long>("INTEGER", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>("TEXT", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DateModified = table.Column<DateTimeOffset>("TEXT", nullable: true),
                    DeactivateReason = table.Column<string>("TEXT", nullable: true),
                    DeactivatedById = table.Column<long>("INTEGER", nullable: true),
                    DeleteReason = table.Column<string>("TEXT", nullable: true),
                    Deleted = table.Column<bool>("INTEGER", nullable: false),
                    DeletedById = table.Column<long>("INTEGER", nullable: true),
                    Description = table.Column<string>("TEXT", nullable: true),
                    IsActive = table.Column<bool>("INTEGER", nullable: false),
                    ModifiedById = table.Column<long>("INTEGER", nullable: true),
                    Name = table.Column<string>("TEXT", nullable: true),
                    ReactivatedById = table.Column<long>("INTEGER", nullable: true),
                    ReactivatedReason = table.Column<string>("TEXT", nullable: true),
                    UndeleteReason = table.Column<string>("TEXT", nullable: true),
                    UndeletedById = table.Column<long>("INTEGER", nullable: true)
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

            migrationBuilder.CreateIndex(
                "IX_ApplicationUsers_AddressId",
                "ApplicationUsers",
                "AddressId");

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
                "IX_Addresses_TownId",
                schema: "Address",
                table: "Addresses",
                column: "TownId");

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

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_ActingUserId",
                "AccountJournalEntry",
                "ActingUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_AccountJournalEntry_ApplicationUsers_JournalId",
                "AccountJournalEntry",
                "JournalId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ActivatedById",
                schema: "Address",
                table: "Addresses",
                column: "ActivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_DeactivatedById",
                schema: "Address",
                table: "Addresses",
                column: "DeactivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_DeletedById",
                schema: "Address",
                table: "Addresses",
                column: "DeletedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ModifiedById",
                schema: "Address",
                table: "Addresses",
                column: "ModifiedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_ReactivatedById",
                schema: "Address",
                table: "Addresses",
                column: "ReactivatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_ApplicationUsers_UndeletedById",
                schema: "Address",
                table: "Addresses",
                column: "UndeletedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_CityBlocks_CityBlockId",
                schema: "Address",
                table: "Addresses",
                column: "CityBlockId",
                principalSchema: "Address",
                principalTable: "CityBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Counties_CountyId",
                schema: "Address",
                table: "Addresses",
                column: "CountyId",
                principalSchema: "Address",
                principalTable: "Counties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Countries_CountryId",
                schema: "Address",
                table: "Addresses",
                column: "CountryId",
                principalSchema: "Address",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Towns_TownId",
                schema: "Address",
                table: "Addresses",
                column: "TownId",
                principalSchema: "Misc",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Addresses_Types_AddressTypeId",
                schema: "Address",
                table: "Addresses",
                column: "AddressTypeId",
                principalSchema: "Address",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ActivatedById",
                "ApplicationRoles",
                "ActivatedById",
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
                "FK_ApplicationRoles_ApplicationUsers_ModifiedById",
                "ApplicationRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationRoles_ApplicationUsers_ReactivatedById",
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
                "FK_ApplicationUsers_Addresses_AddressId",
                "ApplicationUsers",
                "AddressId",
                principalSchema: "Address",
                principalTable: "Addresses",
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
                "FK_ApplicationUsers_ApplicationUsers_ModifiedById",
                "ApplicationUsers",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_ApplicationUsers_ApplicationUsers_ReactivatedById",
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
                "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                "RefreshToken",
                "ApplicationUserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_Addresses_AddressId",
                "UserAddress",
                "AddressId",
                principalSchema: "Address",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ActivatedById",
                "UserAddress",
                "ActivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_CreatedById",
                "UserAddress",
                "CreatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_DeactivatedById",
                "UserAddress",
                "DeactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_DeletedById",
                "UserAddress",
                "DeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ModifiedById",
                "UserAddress",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_ReactivatedById",
                "UserAddress",
                "ReactivatedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_UndeletedById",
                "UserAddress",
                "UndeletedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserAddress_ApplicationUsers_UserId",
                "UserAddress",
                "UserId",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationRoles_RoleId",
                "UserRoles",
                "RoleId",
                "ApplicationRoles",
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
                "FK_UserRoles_ApplicationUsers_ModifiedById",
                "UserRoles",
                "ModifiedById",
                "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_UserRoles_ApplicationUsers_ReactivatedById",
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
    }
}