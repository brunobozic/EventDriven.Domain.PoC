using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Domain.DomainEntities.Audit;
using EventDriven.Domain.PoC.Domain.DomainEntities.OutboxPattern;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.Audit;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext.Interfaces;
using EventDriven.Domain.PoC.Repository.EF.Extensions;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;

namespace EventDriven.Domain.PoC.Repository.EF.DatabaseContext
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly List<AuditTrail> _auditList = new();
        private readonly List<EntityEntry> _list = new();

        private DbAuditTrailFactory _auditFactory;
        private DbContextOptions options;

        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<InternalCommand> InternalCommands { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<Role> ApplicationRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbContextOptions<ApplicationDbContext> Options { get; }

        public ApplicationDbContext UnderlyingContext()
        {
            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ForSqlServerUseSequenceHiLo("DBSequenceHiLo");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<UserRole>();
            modelBuilder.Entity<UserAddress>();
            modelBuilder.Entity<AccountJournalEntry>();
            modelBuilder.Entity<Address>();
            modelBuilder.Entity<AddressType>();
            modelBuilder.SetUpSoftDeletableColumnDefaultValue();
            modelBuilder.DisableCascadeDelete();
            modelBuilder.LoadAllEntityConfigurations();
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException(
                "Use only the SaveChangesAsync() because synchronous saving is not supported!");
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new())
        {
            _auditList.Clear();
            _list.Clear();
            _auditFactory = new DbAuditTrailFactory(this);
            var changes = 0;

            // Do the audit trails
            var currentDateTime = DateTime.Now;

            // TODO: fetch user Id from the facade
            var currentApplicationUserId = await ApplicationUsers
                .Where(user => user.UserName == Consts.SYSTEM_USER_USERNAME).Select(i => i.Id)
                .SingleOrDefaultAsync(cancellationToken);

            try
            {
                // Do the soft deletes
                foreach (var deletableEntity in ChangeTracker.Entries<ISoftDeletable>())
                {
                    if (deletableEntity.State != EntityState.Deleted) continue;

                    // We need to set this to unchanged here, because setting it to modified seems to set ALL of its fields to modified
                    deletableEntity.State = EntityState.Unchanged;

                    // This will set the entity's state to modified for the next time we query the ChangeTracker
                    deletableEntity.Entity.Deleted = true;

                    deletableEntity.Entity.DateDeleted = currentDateTime;
                    deletableEntity.Entity.DeletedBy = currentApplicationUserId;

                    deletableEntity.State = EntityState.Modified;
                    // Now, add soft deleted entity to full audit list...
                    var audit = _auditFactory.GetAudit(deletableEntity);
                    _auditList.Add(audit);
                    _list.Add(deletableEntity);
                }

                // Deal with the "made active" / "made inactive" / "will expire at datetime" entities here...
                foreach (var activeItem in ChangeTracker.Entries<IDeactivatableEntity>())
                {
                    if (activeItem.State != EntityState.Deleted) continue;

                    if (activeItem.Entity.IsActive)
                    {
                        if (activeItem.Entity.ActiveFrom == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                            activeItem.Entity.ActiveFrom = DateTime.Now;
                        if (activeItem.Entity.ActiveTo == null || activeItem.Entity.ActiveFrom == DateTime.MinValue)
                        {
                            // do not set in advance !!
                        }
                    }
                }

                #region Full audit trail

                try
                {
                    var entityList =
                        ChangeTracker.Entries<IAuditTrail>()
                            .Where(p => p.State == EntityState.Added
                                        || p.State == EntityState.Deleted
                                        || p.State == EntityState.Modified);

                    foreach (var entity in entityList)
                    {
                        var audit = _auditFactory.GetAudit(entity);
                        _auditList.Add(audit);
                        _list.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Debug.WriteLine(e);
                    Log.Fatal("Unable to create an audit trail.", e);
                }

                #endregion Full audit trail

                //foreach (var auditableEntity in ChangeTracker.Entries<IAmFullyAudited>())
                //{
                //    if (auditableEntity.State != EntityState.Added &&
                //        auditableEntity.State != EntityState.Modified) continue;

                //    // Adding or modifying - update the edited audit trails
                //    auditableEntity.Entity.DateModified = currentDateTime;
                //    auditableEntity.Entity.ModifiedBy = currentApplicationUserId.ToString();

                //    switch (auditableEntity.State)
                //    {
                //        case EntityState.Added:

                //            // Adding - set the created audit trails
                //            auditableEntity.Entity.DateCreated = currentDateTime;
                //            auditableEntity.Entity.CreatedBy = currentApplicationUserId.ToString();

                //            break;

                //        case EntityState.Modified:

                //            // Modified (or deleted from above) - ensure that the created fields are not being modified
                //            var fullName = auditableEntity.Entity.GetType().Name;
                //            if (fullName != null && !fullName.Equals("ApplicationUser"))
                //            {
                //                //if (auditableEntity.Property(p => p.CreatedDate).IsModified ||
                //                //    auditableEntity.Property(p => p.CreatedById).IsModified)
                //                //    throw new DbEntityValidationException(
                //                //        $"Attempt to change created audit trails on a modified {auditableEntity.Entity.GetType().FullName}");
                //            }

                //            break;
                //    }
                //}

                foreach (var auditableEntity in ChangeTracker.Entries<IDeletionAuditedEntity>())
                    if (auditableEntity.State == EntityState.Deleted)
                    {
                        auditableEntity.Entity.DateDeleted = currentDateTime;
                        auditableEntity.Entity.DeletedById = currentApplicationUserId;
                    }

                foreach (var auditableEntity in ChangeTracker.Entries<IModificationAuditedEntity>())
                    if (auditableEntity.State == EntityState.Modified)
                    {
                        auditableEntity.Entity.DateModified = currentDateTime;
                        auditableEntity.Entity.ModifiedById = currentApplicationUserId;
                    }

                foreach (var auditableEntity in ChangeTracker.Entries<ICreationAuditedEntity>())
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.DateCreated = currentDateTime;
                        if (!auditableEntity.Entity.IsSeed)
                            auditableEntity.Entity.CreatedById = currentApplicationUserId;
                    }

                changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                if (_auditList.Count > 0)
                {
                    var i = 0;
                    foreach (var audit in _auditList)
                    {
                        if (audit.Actions == AuditActions.I.ToString())
                        {
                            audit.TableIdValue = _auditFactory.GetKeyValueLong(_list[i]);
                            audit.TableIdValueGuid = _auditFactory.GetKeyValueGuid(_list[i]);
                        }

                        await AuditTrail.AddAsync(audit, cancellationToken);
                        i++;
                    }

                    await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                }
            }
            //catch (DbEntityValidationException entityException)
            //{
            //    var errors = entityException.EntityValidationErrors;
            //    var result = new StringBuilder();
            //    var allErrors = new List<ValidationResult>();

            //    foreach (var error in errors)
            //        foreach (var validationError in error.ValidationErrors)
            //        {
            //            result.AppendFormat(
            //                "\r\n  Entity of type {0} has validation error \"{1}\" for property {2}.\r\n",
            //                error.Entry.Entity.GetType(), validationError.ErrorMessage, validationError.PropertyName);
            //            var domainEntity = error.Entry.Entity as DomainEntity<int>;
            //            if (domainEntity != null)
            //                result.Append(domainEntity.IsTransient()
            //                    ? "  This entity was added in this session.\r\n"
            //                    : $"  The Id of the entity is {domainEntity.Id}.\r\n");
            //            allErrors.Add(new ValidationResult(validationError.ErrorMessage,
            //                new[] { validationError.PropertyName }));
            //        }

            //    throw new ModelValidationException(result.ToString(), entityException, allErrors);
            //}
            catch (DbUpdateConcurrencyException ex
            ) // This will fire only for entities that have the [RowVersion] property implemented...
            {
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;
                var databaseEntry = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseEntry == null)
                {
                    // The entity was deleted by another user...
                }
                else
                {
                    // Otherwise create the object based on what is now in the db...
                    var databaseValues = databaseEntry.ToObject();
                }

                //	if (databaseValues.Name != clientValues.Name)
                //		ModelState.AddModelError("Name", "Current value: "
                //			+ databaseValues.Name);
                //	if (databaseValues.Budget != clientValues.Budget)
                //		ModelState.AddModelError("Budget", "Current value: "
                //			+ String.Format("{0:c}", databaseValues.Budget));
                //	if (databaseValues.StartDate != clientValues.StartDate)
                //		ModelState.AddModelError("StartDate", "Current value: "
                //			+ String.Format("{0:d}", databaseValues.StartDate));
                //	if (databaseValues.InstructorID != clientValues.InstructorID)
                //		ModelState.AddModelError("InstructorID", "Current value: "
                //			+ db.Instructors.Find(databaseValues.InstructorID).FullName);
                //	ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                //		+ "was modified by another user after you got the original value. The "
                //		+ "edit operation was canceled and the current values in the database "
                //		+ "have been displayed. If you still want to edit this record, click "
                //		+ "the Save button again. Otherwise click the Back to List hyperlink.");
                //	departmentToUpdate.RowVersion = databaseValues.RowVersion;
                //}

                // Update the values of the entity that failed to save from the store 
                await ex.Entries.Single().ReloadAsync(cancellationToken);

                var result = new StringBuilder();
                result.Append("The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed.");

                // throw new DbUpdateConcurrencyException(result.ToString(), ex.Data);
            }

            return changes;
        }

        #region ctor

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions options, bool fromFactory) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.options = options;
        }

        #endregion ctor
    }
}