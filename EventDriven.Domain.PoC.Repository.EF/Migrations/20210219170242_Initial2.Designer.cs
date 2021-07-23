﻿// <auto-generated />
using System;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventDriven.Domain.PoC.Repository.EF.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210219170242_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.AccountJournalEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TheUserHasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DeletedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccountJournalEntry");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CreateddBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TheUserHasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DeletedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActivatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CreateddBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<int>("DectivatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("TheUserHasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DeletedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastVerificationFailureDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Oib")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetMsg")
                        .HasColumnType("TEXT");

                    b.Property<int>("PromotedToAdminByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResetToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LatestVerificationFailureMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailVerificationToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VerificationTokenExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Verified")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationUserApplicationRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TheUserHasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DeletedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Audit.AuditTrail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Actions")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TheUserHasBeenDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DeletedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LastModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NewData")
                        .HasColumnType("TEXT");

                    b.Property<string>("OldData")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TableIdValue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TableName")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DbAuditTrail", "Audit");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.OutboxPattern.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("InternalCommands");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.OutboxPattern.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ApplicationUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("TEXT");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.AccountJournalEntry", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.ApplicationUser", "User")
                        .WithMany("JournalEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationUserApplicationRole", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.RefreshToken", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.ApplicationUser", "ApplicationUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.ApplicationUser", b =>
                {
                    b.Navigation("JournalEntries");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
