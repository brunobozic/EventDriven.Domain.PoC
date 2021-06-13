namespace EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters
{
    public partial class HttpGlobalExceptionFilter
    {
        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}