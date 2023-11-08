namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors
{
    public class ValidationError
    {
        #region Constants

        public static class Type
        {
            public static string Domain = "domain";
            public static string Input = "input";
        }

        #endregion Constants

        #region Properties

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public object Source { get; set; }

        #endregion Properties

        #region Constructor

        public ValidationError()
        {
        }

        public ValidationError(string type, string code, object source, string message)
        {
            ErrorType = type;
            ErrorCode = code;
            Source = source;
            ErrorMessage = message;
        }

        #endregion Constructor
    }
}