using System;
using System.ComponentModel;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers
{
    public static class EnumExtensions
    {
        public static string GetOracleParamName<TEnum>(this TEnum @enum)
        {
            var info = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[]) info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?[0].Description ?? @enum.ToString();
        }

        public static string GetDescriptionString<TEnum>(this TEnum @enum)
        {
            try
            {
                var info = @enum.GetType().GetField(@enum.ToString());
                var attributes = (DescriptionAttribute[]) info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes?[0].Description ?? @enum.ToString();
            }
            catch (Exception)
            {
                return "Nepoznato";
            }
        }

        public static T GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T) field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T) field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }
    }

    public static class EnumHelper
    {
        public static T GetEnumValue<T>(this string str) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new Exception("T must be an Enumeration type.");
            T val;
            return Enum.TryParse(str, true, out val) ? val : default;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new Exception("T must be an Enumeration type.");

            return (T) Enum.ToObject(enumType, intValue);
        }
    }
}