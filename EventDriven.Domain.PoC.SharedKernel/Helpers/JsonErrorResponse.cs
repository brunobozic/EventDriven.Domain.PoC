namespace EventDriven.Domain.PoC.SharedKernel.Helpers
{
    public class Startup
    {
        public class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public string DeveloperMessage { get; set; }
            public string User { get; set; }
        }
    }
}