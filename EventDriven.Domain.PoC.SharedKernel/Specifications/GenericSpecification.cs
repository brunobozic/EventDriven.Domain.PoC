using System;
using System.Linq.Expressions;

namespace EventDriven.Domain.PoC.SharedKernel.Specifications
{
    public abstract class Specification<T>
    {
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();
    }
}