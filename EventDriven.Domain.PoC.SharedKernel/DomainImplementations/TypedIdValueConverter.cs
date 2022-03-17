using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations
{
    public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
        where TTypedIdValue : TypedIdValueBase
    {
        public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Value, value => Create(value), mappingHints)
        {
        }

        private static TTypedIdValue Create(Guid id)
        {
            return Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue;
        }
    }
}