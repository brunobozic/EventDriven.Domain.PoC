namespace EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters;

public partial class HttpGlobalExceptionFilter
{
    private class JsonErrorResponse
    {
        public object DeveloperMessage { get; set; }
        public string[] Messages { get; set; }
    }
}