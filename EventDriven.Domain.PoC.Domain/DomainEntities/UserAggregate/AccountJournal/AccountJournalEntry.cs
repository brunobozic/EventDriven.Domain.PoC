using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.DomainBaseAbstractions;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal
{
    public class AccountJournalEntry : JournalEntityOfT<Guid>
    {
        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public void AttachUser(User user)
        {
            UserActedUpon = user;
            UserNameActedUpon = user.UserName;
            EmailActedUpon = user.Email;
            UserActedUponId = user.Id;
        }

        public void AttachActingUser(User activatedBy)
        {
            if (activatedBy != null)
            {
                ActingUser = activatedBy;
                ActingUserId = activatedBy.Id;
                ActingEmail = activatedBy.Email;
                ActingUserName = activatedBy.UserName;
            }
        }

        #endregion Public Methods

        #region ctor

        private AccountJournalEntry()
        {
        }

        public AccountJournalEntry(string msg)
        {
            Message = msg;
        }

        #endregion ctor

        #region Public Props

        public string EmailActedUpon { get; private set; }
        public string UserNameActedUpon { get; private set; }

        public string Message { get; }
        public string ActingUserName { get; private set; }
        public string ActingEmail { get; private set; }
        public DateTimeOffset? Seen { get; private set; }

        #endregion Public Props

        #region Navigation

        public virtual User UserActedUpon { get; private set; }
        public virtual User ActingUser { get; private set; }

        #endregion Navigation

        #region FK

        public Guid?
            ActingUserId
        {
            get;
            private set;
        } // nullable because of an annoying issue with not having this information when the user is not yet authenticated (registered)

        public Guid UserActedUponId { get; private set; }

        #endregion FK
    }
}