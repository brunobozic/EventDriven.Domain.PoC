using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace EventDriven.Domain.PoC.Api.Rest.SwaggerOverrides
{
    public class UserRegistrationCustomOperationFilter : IOperationFilter
    {
        private Random random = new Random();
        private string sharedPassword;

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

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Handle the request body
            if (operation.RequestBody != null)
            {
                foreach (var content in operation.RequestBody.Content)
                {
                    // Check if the content type is JSON
                    if (content.Key.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var schema = content.Value.Schema;
                        if (schema != null && schema.Properties != null)
                        {
                            foreach (var property in schema.Properties)
                            {
                                // Apply custom logic based on property names
                                switch (property.Key.ToLower())
                                {
                                    case "dateOfBirth":
                                        property.Value.Example = new OpenApiString(RandomPastDate());
                                        break;

                                    case "email":
                                        property.Value.Example = new OpenApiString($"user{random.Next(1000, 9999)}@example.com");
                                        break;

                                    case "firstName":
                                    case "lastName":
                                    case "userName":
                                        property.Value.Example = new OpenApiString(RandomString(10));
                                        break;

                                    case "oib":
                                        property.Value.Example = new OpenApiString(random.Next(100000, 999999).ToString());
                                        break;

                                    case "password":
                                    case "confirmPassword":
                                        if (string.IsNullOrEmpty(sharedPassword))
                                        {
                                            sharedPassword = RandomString(12);
                                        }
                                        property.Value.Example = new OpenApiString(sharedPassword);
                                        break;

                                    case "acceptTerms":
                                        property.Value.Example = new OpenApiBoolean(true);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}