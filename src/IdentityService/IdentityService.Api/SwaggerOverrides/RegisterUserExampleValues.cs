using System;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;
using Swashbuckle.AspNetCore.Filters;

namespace IdentityService.Api.SwaggerOverrides;

public class RegisterUserExampleValues : IExamplesProvider<RegisterUserRequest>
{
    private readonly Random random = new();
    private string pwd = string.Empty;

    public RegisterUserRequest GetExamples()
    {
        var pwd = GenerateRandomPassword();
        return new RegisterUserRequest(
            GenerateRandomEmail(),
            pwd,
            RandomPastDate(),
            GetRandomName(),
            GetRandomSurname(),
            pwd,
            GenerateRandomUsername(),
            GenerateRandomOib(),
            true
        );
    }

    private string GenerateRandomEmail()
    {
        var domains = new[] { "example.com", "mail.com", "test.org" };
        return $"{GenerateRandomUsername()}@{domains[random.Next(domains.Length)]}";
    }

    private string GetRandomName()
    {
        var names = new[]
        {
            "John", "Jane", "Mark", "Mary", "Steve", "Sara", "Emily", "Michael",
            "Emma", "James", "Olivia", "William", "Ava", "Benjamin", "Sophia",
            "Lucas", "Isabella", "Henry", "Charlotte", "Alexander", "Amelia",
            "Mason", "Mia", "Ethan", "Harper", "Evelyn", "Abigail", "Ella", "Scarlett",
            "Grace", "Chloe", "Camila", "Penelope", "Riley", "Layla", "Lillian", "Nora",
            "Zoey", "Mila", "Aubrey", "Hannah", "Lily", "Addison", "Eleanor", "Natalie",
            "Luna", "Savannah", "Brooklyn", "Leah", "Zoe", "Stella", "Hazel", "Ellie",
            "Paisley", "Audrey", "Skylar", "Violet", "Claire", "Bella", "Aurora", "Lucy",
            "Alice", "Gabriella", "Madison", "Avery", "Elena", "Victoria", "Sophie", "Aria",
            "Grace", "Ellie", "Ivy", "Kinsley", "Naomi", "Eva", "Maya", "Ruby", "Ariel",
            "Hailey", "Alexa", "Annabelle", "Sienna", "Cora", "Julia", "Maria", "Valentina",
            "Nova", "Clara", "Vivian", "Reagan", "Mackenzie", "Madeline"
        };

        return $"{names[random.Next(names.Length)]}{random.Next(10, 100)}";
    }

    private string GetRandomSurname()
    {
        var surnames = new[]
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
            "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson",
            "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson",
            "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker",
            "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill",
            "Flores", "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell",
            "Mitchell", "Carter", "Roberts", "Gomez", "Phillips", "Evans", "Turner", "Diaz",
            "Parker", "Cruz", "Edwards", "Collins", "Reyes", "Stewart", "Morris", "Morales",
            "Murphy", "Cook", "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper", "Peterson",
            "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox", "Ward", "Richardson",
            "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza",
            "Ruiz", "Hughes", "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers",
            "Long", "Ross", "Foster", "Jimenez", "Powell", "Jenkins", "Perry", "Russell",
            "Sullivan", "Bell", "Coleman", "Butler", "Henderson", "Barnes", "Gonzales",
            "Fisher", "Vasquez", "Simmons", "Romero", "Jordan", "Patterson", "Alexander",
            "Hamilton", "Graham", "Reynolds", "Griffin", "Wallace", "Moreno", "West",
            "Cole", "Hayes", "Bryant", "Herrera", "Gibson", "Ellis", "Tran", "Medina",
            "Aguilar", "Stevens", "Murray", "Ford", "Castro", "Marshall", "Owens", "Harrison"
        };

        return $"{surnames[random.Next(surnames.Length)]}{random.Next(10, 100)}";
    }

    private string GenerateRandomUsername()
    {
        return $"user{random.Next(1000, 9999)}";
    }

    private string GenerateRandomPassword()
    {
        // Simple password generator; consider a more complex approach for real-world scenarios
        return $"Pass{random.Next(1000, 9999)}!";
    }

    private string GenerateRandomOib()
    {
        // Assuming OIB is a numeric identifier; adjust as needed for your specific requirements
        return random.Next(100000, 999999).ToString();
    }

    private DateTime RandomPastDate()
    {
        var daysAgo = random.Next(1, 10000);
        return DateTime.Now.AddDays(-daysAgo);
    }
}