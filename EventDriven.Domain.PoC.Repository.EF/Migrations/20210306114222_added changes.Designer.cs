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
    [Migration("20210306114222_added changes")]
    partial class addedchanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.AccountJournalEntry", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccountJournalEntry");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Audit.AuditTrail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Actions")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ActivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeactivateReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DeactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NewData")
                        .HasColumnType("TEXT");

                    b.Property<string>("OldData")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ReactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReactivatedReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TableIdValue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TableName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UndeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("UndeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeactivatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("ModifiedById");

                    b.HasIndex("ReactivatedById");

                    b.HasIndex("UndeletedById");

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
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ApplicationUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
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

                    b.HasIndex("LastModifiedById");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ActivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CreateddBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeactivateReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DeactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ReactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReactivatedReason")
                        .HasColumnType("TEXT");

                    b.Property<string>("UndeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("UndeletedById")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeactivatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("LastModifiedById");

                    b.HasIndex("ModifiedById");

                    b.HasIndex("ReactivatedById");

                    b.HasIndex("UndeletedById");

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ActivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActivatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<string>("BasicRole")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeactivateReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DeactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeactivatedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastVerificationFailureDate")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ModifiedById")
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

                    b.Property<long?>("ReactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReactivatedReason")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResetToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UndeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("UndeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerificationFailureLatestMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VerificationTokenExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Verified")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById")
                        .IsUnique();

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeactivatedById")
                        .IsUnique();

                    b.HasIndex("DeletedById")
                        .IsUnique();

                    b.HasIndex("LastModifiedById")
                        .IsUnique();

                    b.HasIndex("ModifiedById")
                        .IsUnique();

                    b.HasIndex("ReactivatedById")
                        .IsUnique();

                    b.HasIndex("UndeletedById")
                        .IsUnique();

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ActivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("ActiveFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ActiveTo")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateDeleted")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeactivateReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DeactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ModifiedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long?>("ReactivatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReactivatedReason")
                        .HasColumnType("TEXT");

                    b.Property<long>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UndeleteReason")
                        .HasColumnType("TEXT");

                    b.Property<long?>("UndeletedById")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById")
                        .IsUnique();

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeactivatedById")
                        .IsUnique();

                    b.HasIndex("DeletedById")
                        .IsUnique();

                    b.HasIndex("LastModifiedById")
                        .IsUnique();

                    b.HasIndex("ModifiedById")
                        .IsUnique();

                    b.HasIndex("ReactivatedById")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UndeletedById")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.AccountJournalEntry", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "User")
                        .WithMany("JournalEntries")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LastModifiedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Audit.AuditTrail", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ActivatedBy")
                        .WithMany()
                        .HasForeignKey("ActivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ReactivatedBy")
                        .WithMany()
                        .HasForeignKey("ReactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "UndeletedBy")
                        .WithMany()
                        .HasForeignKey("UndeletedById");

                    b.Navigation("ActivatedBy");

                    b.Navigation("CreatedBy");

                    b.Navigation("DeactivatedBy");

                    b.Navigation("DeletedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("ModifiedBy");

                    b.Navigation("ReactivatedBy");

                    b.Navigation("UndeletedBy");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.RefreshToken", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ApplicationUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.Navigation("ApplicationUser");

                    b.Navigation("LastModifiedBy");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Role", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ActivatedBy")
                        .WithMany()
                        .HasForeignKey("ActivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeactivatedBy")
                        .WithMany()
                        .HasForeignKey("DeactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithMany()
                        .HasForeignKey("LastModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ReactivatedBy")
                        .WithMany()
                        .HasForeignKey("ReactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "UndeletedBy")
                        .WithMany()
                        .HasForeignKey("UndeletedById");

                    b.Navigation("ActivatedBy");

                    b.Navigation("CreatedBy");

                    b.Navigation("DeactivatedBy");

                    b.Navigation("DeletedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("ModifiedBy");

                    b.Navigation("ReactivatedBy");

                    b.Navigation("UndeletedBy");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.User", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ActivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "ActivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeactivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "DeactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeletedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "DeletedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ModifiedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "ModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ReactivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "ReactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "UndeletedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.User", "UndeletedById");

                    b.Navigation("ActivatedBy");

                    b.Navigation("CreatedBy");

                    b.Navigation("DeactivatedBy");

                    b.Navigation("DeletedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("ModifiedBy");

                    b.Navigation("ReactivatedBy");

                    b.Navigation("UndeletedBy");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.UserRole", b =>
                {
                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ActivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "ActivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeactivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "DeactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "DeletedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "DeletedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "LastModifiedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "LastModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ModifiedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "ModifiedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "ReactivatedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "ReactivatedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "UndeletedBy")
                        .WithOne()
                        .HasForeignKey("EventDriven.Domain.PoC.Domain.Domain.UserRole", "UndeletedById");

                    b.HasOne("EventDriven.Domain.PoC.Domain.Domain.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivatedBy");

                    b.Navigation("CreatedBy");

                    b.Navigation("DeactivatedBy");

                    b.Navigation("DeletedBy");

                    b.Navigation("LastModifiedBy");

                    b.Navigation("ModifiedBy");

                    b.Navigation("ReactivatedBy");

                    b.Navigation("Role");

                    b.Navigation("UndeletedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("EventDriven.Domain.PoC.Domain.Domain.User", b =>
                {
                    b.Navigation("JournalEntries");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
