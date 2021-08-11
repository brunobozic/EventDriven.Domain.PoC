using System;
using System.Linq.Expressions;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.Specifications;

namespace EventDriven.Domain.PoC.Domain.ExampleSpecifications
{
    public sealed class ValidUserSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression()
        {
            return x => string.IsNullOrWhiteSpace(x.UserName) == false
                        && string.IsNullOrWhiteSpace(x.FirstName) == false
                        && string.IsNullOrWhiteSpace(x.LastName) == false
                        && string.IsNullOrWhiteSpace(x.Email) == false
                        && x.IsDraft == false
                        && x.TheUserHasBeenDeleted == false
                        && x.Active
                        && x.Email.Contains("@")
                //&& x.Email.EndsWith(".edu")
                ;
        }
    }
}