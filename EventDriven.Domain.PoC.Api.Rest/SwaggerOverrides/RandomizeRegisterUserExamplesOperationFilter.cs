using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace EventDriven.Domain.PoC.Api.Rest.SwaggerOverrides
{
    public class RandomizeRegisterUserExamplesOperationFilter : IOperationFilter
    {
        private Random random = new Random();
        private string sharedPassword;

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var parameter in operation.Parameters)
            {
                if (parameter.Schema != null)
                {
                    switch (parameter.Name.ToLower())
                    {
                        case "dateOfBirth":
                            parameter.Schema.Example = new OpenApiString(RandomPastDate());
                            break;

                        case "email":
                            parameter.Schema.Example = new OpenApiString($"user{random.Next(1000, 9999)}@example.com");
                            break;

                        case "firstName":
                        case "lastName":
                        case "userName":
                            parameter.Schema.Example = new OpenApiString(RandomString(10));
                            break;

                        case "oib":
                            parameter.Schema.Example = new OpenApiString(random.Next(100000, 999999).ToString());
                            break;

                        case "password":
                        case "confirmPassword":
                            if (string.IsNullOrEmpty(sharedPassword))
                            {
                                sharedPassword = RandomString(12);
                            }
                            parameter.Schema.Example = new OpenApiString(sharedPassword);
                            break;

                        case "acceptTerms":
                            parameter.Schema.Example = new OpenApiBoolean(true);
                            break;
                    }
                }
            }
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string RandomPastDate()
        {
            var daysAgo = random.Next(1, 10000);
            return DateTime.Now.AddDays(-daysAgo).ToString("o");
        }
    }
}