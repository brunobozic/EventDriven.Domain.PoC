using IdentityService.Domain.DomainEntities.DomainExceptions;
using SharedKernel.DomainBaseAbstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

[Table("Countries", Schema = "Codebook")]
public class CountryCodebook : DomainEntity<long>
{
    [Required] public string Name { get; set; }

    [Required] public string LocalName { get; set; }

    [Required] public string ISO_3166_ALPHA_2 { get; set; }

    [Required] public string ISO_3166_ALPHA_3 { get; set; }

    [Required] public string ISO_3166_NUMERIC { get; set; }


    public User ActivatedBy { get; set; }
    public long Id { get; set; }

    public void SetActive(DateTimeOffset from, DateTimeOffset to, User activator)
    {
        if (!Deleted)
        {
            IsActive = true;
            ActiveFrom = from;
            ActiveTo = to;
            ActivatedBy = activator;
        }
        else
        {
            throw new DomainException("Cannot set active to a deleted item.");
        }
    }

    public void SetInActive(User deactivator)
    {
        if (!Deleted)
            IsActive = false;
        else
            throw new DomainException("Cannot set inactive to a deleted item.");
    }

    public void SetCreator(User creator)
    {
        CreatedBy = creator.Id;
    }

    public void Delete(User deletor)
    {
        if (!Deleted)
        {
            DeletedBy = deletor.Id;
            Deleted = true;
        }
        else
        {
            throw new DomainException("Cannot delete a deleted item.");
        }
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}