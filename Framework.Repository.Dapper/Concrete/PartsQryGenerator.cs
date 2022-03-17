using Framework.Repository.Dapper.Contracts;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.Repository.Dapper.Concrete
{
    public class PartsQryGenerator<TEntity> : IPartsQryGenerator<TEntity> where TEntity : class
    {
        private readonly string _characterParameter;
        private readonly string[] _propertiesNames;
        private readonly string _typeName;

        public PartsQryGenerator(char characterParameter = '@')
        {
            var type = typeof(TEntity);

            _characterParameter = characterParameter.ToString();

            var properties = type.GetProperties();
            _propertiesNames = properties.Where(a => !IsComplexType(a)).Select(a => a.Name).ToArray();
            _typeName = type.Name;
        }

        public string GeneratePartInsert(string identityField = null)
        {
            var sb = new StringBuilder($"INSERT INTO {_typeName} (");

            var propertiesNamesDef = _propertiesNames.Where(a => a != identityField).ToArray();

            var camps = string.Join(",", propertiesNamesDef);

            sb.Append($"{camps}) VALUES (");

            var parametersCampsCol = propertiesNamesDef.Select(a => $"{_characterParameter}{a}").ToArray();

            var campsParameter = string.Join(",", parametersCampsCol);

            sb.Append($"{campsParameter})");

            var result = sb.ToString();

            return result;
        }

        public string GenerateSelect()
        {
            var sb = new StringBuilder("SELECT ");

            var separator = $",{Environment.NewLine}";

            var selectPart = string.Join(separator, _propertiesNames);

            sb.AppendLine(selectPart);

            var fromPart = $"FROM {_typeName}";

            sb.Append(fromPart);

            var result = sb.ToString();

            return result;
        }

        public string GenerateDelete(object parameters)
        {
            ParameterValidator.ValidateObject(parameters, nameof(parameters));

            var where = GenerateWhere(parameters);

            var result = $"DELETE FROM {_typeName} {where} ";

            return result;
        }

        public string GenerateUpdate(object pks)
        {
            ParameterValidator.ValidateObject(pks, nameof(pks));

            var pksFields = pks.GetType().GetProperties().Select(a => a.Name).ToArray();

            var sb = new StringBuilder($"UPDATE {_typeName} SET ");

            var propertiesNamesDef = _propertiesNames.Where(a => !pksFields.Contains(a)).ToArray();

            var propertiesSet = propertiesNamesDef.Select(a => $"{a} = {_characterParameter}{a}").ToArray();

            var strSet = string.Join(",", propertiesSet);

            var where = GenerateWhere(pks);

            sb.Append($" {strSet} {where} ");

            var result = sb.ToString();

            return result;
        }

        public string GenerateSelect(object fieldsFilter)
        {
            ParameterValidator.ValidateObject(fieldsFilter, nameof(fieldsFilter));

            var initialSelect = GenerateSelect();

            var where = GenerateWhere(fieldsFilter);

            var result = $" {initialSelect} {where}";

            return result;
        }

        private string GenerateWhere(object filtersPKs)
        {
            ParameterValidator.ValidateObject(filtersPKs, nameof(filtersPKs));

            var filtersPksFields = filtersPKs.GetType().GetProperties().Select(a => a.Name).ToArray();

            if (!filtersPksFields?.Any() ?? true)
                throw new ArgumentException("Parameter filtersPks isn't valid. This parameter must be a class type",
                    nameof(filtersPKs));

            var propertiesWhere = filtersPksFields.Select(a => $"{a} = {_characterParameter}{a}").ToArray();
            var strWhere = string.Join(" AND ", propertiesWhere);

            var result = $" WHERE {strWhere} ";

            return result;
        }

        private static bool IsComplexType(PropertyInfo propertyInfo)
        {
            bool result;

            result = propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.Name != "String" ||
                     propertyInfo.PropertyType.IsInterface;

            return result;
        }
    }
}