using System;

namespace EventDriven.Domain.PoC.SharedKernel.Attributes
{
    /// <summary>
    ///     Marks property to have unique value in DB.
    /// </summary>
    /// <remarks>
    ///     If multiple properties need have joint unique constratin use same compoundName.
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute()
        {
            CompundName = string.Empty;
        }

        public UniqueAttribute(string compoundName)
        {
            if (string.IsNullOrEmpty(compoundName))
                throw new ArgumentException("Compound name cannot be null or empty string.");

            CompundName = compoundName;
        }

        public virtual string CompundName { get; }
    }
}