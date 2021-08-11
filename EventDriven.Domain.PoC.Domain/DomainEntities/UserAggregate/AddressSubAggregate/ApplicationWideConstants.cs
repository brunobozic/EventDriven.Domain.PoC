namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate
{
    public class ApplicationWideConstants
    {
        public const int DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES = 100;

        public const int DEFAULT_ACTIVETO_VALUE_FOR_USER_REGISTRATIONS = 50;

        public const int DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS = 100;

        public const string SYSTEM_USER = "2da4d020-5ac7-453b-a28a-e621aeb9c109";

        public const string UserEmail = "test.admin@gmail.com";
        public const string UserEmail2 = "bruno.bozic@gmail.com";
        public const string UserEmail3 = "test.admin3@gmail.com";
        public const string UserEmail4 = "test.admin4@gmail.com";
        public const string AdministratorRoleName = "Administrator";
        public const string Guest = "Guest";

        public static string SYSTEM_USER_OIB = "1111111";
        public static string SYSTEM_USER_PASSWORD = "SystemUser";

        public static string SYSTEM_USER_USERNAME = "SystemUser";
        public static int SYSTEM_USER_ACTIVE_TO_ADD_YEARS = 120;

        public static int DEMO_USER_ACTIVE_TO_ADD_YEARS = 1;
        public static double VERIFICATION_TOKEN_EXPIRES_IN_HOURS = 8;

        public static double REFREST_TOKEN_TTL_HOURS = 8;
    }
}