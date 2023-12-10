namespace SharedKernel.ActiveDirectory;
//public class AdUserProvider : IUserProvider
//{
//    public AdUser CurrentUser { get; set; }
//    public IWebHostEnvironment _env { get; set; }
//    public IConfiguration _config { get; }
//    public bool Initialized { get; set; }

//    public AdUserProvider(IConfiguration config, IWebHostEnvironment env)
//    {
//        _env = env;
//        _config = config;
//    }

//    public async Task Create(HttpContext context, IConfiguration config, IWebHostEnvironment env)
//    {
//        CurrentUser = await GetAdUser(context.User.Identity, config);

//        if (CurrentUser == null) throw new DomainException("AD User not found, you are not authorized.");

//        Initialized = true;
//    }

//    public Task<AdUser> GetAdUser(IIdentity identity, IConfiguration config)
//    {
//        return Task.Run(() =>
//        {
//            try
//            {
//                string OU = "OU=Development OU,OU=Users OU,DC=DEV,DC=Teched,DC=HR";

//                string dom = "DEV.Teched.HR";

//                string userAD = "LDAP://DEV-011-DC.DEV.Teched.HR/OU=iCIS";

//                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, dom, OU))
//                {
//                    UserPrincipal principal = new UserPrincipal(context);

//                    if (context != null)
//                    {
//                        var useAd = true;
//                        bool? configValue = null;

//                        try
//                        {
//                            configValue = config.GetSection("MyConfigurationValues").GetValue<bool>("UseActiveDirectory");
//                        }
//                        catch (Exception ex)
//                        {
//                            configValue = null;
//                        }

//                        if (configValue.HasValue) useAd = configValue.Value;

//                        if (useAd && (_env.EnvironmentName == "Development" || _env.EnvironmentName == "LocalDevelopment" || _env.EnvironmentName == "RazvojnaMup"))
//                        {
//                            Log.Warning(" ======> (Important!) Using a fixed demo AD user [ {1} ] becasue our environment is: [ {0} ]", _env.EnvironmentName, "TECHED-DEV\\iCIS_Djelatnik_FinIs+");
//                            principal = UserPrincipal.FindByIdentity(context, IdentityType.SamapplicationUserName, "TECHED-DEV\\iCIS_Djelatnik_FinIs");
//                        }
//                        else if (useAd && (_env.EnvironmentName == "Production"))
//                        {
//                            principal = UserPrincipal.FindByIdentity(context, IdentityType.SamapplicationUserName, identity.Name);
//                        }
//                    }

//                    if (principal == null) return null;

//                    return AdUser.CastToAdUser(principal);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new DomainException("Error retrieving AD User", ex);
//            }
//        });
//    }

//    public Task<AdUser> GetAdUser(string samapplicationUserName)
//    {
//        return Task.Run(() =>
//        {
//            try
//            {
//                PrincipalContext context = new PrincipalContext(ContextType.Domain);
//                UserPrincipal principal = new UserPrincipal(context);

//                if (context != null)
//                {
//                    principal = UserPrincipal.FindByIdentity(context, IdentityType.SamapplicationUserName, samapplicationUserName);
//                }

//                return AdUser.CastToAdUser(principal);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error retrieving AD User", ex);
//            }
//        });
//    }

//    public Task<AdUser> GetAdUser(Guid guid)
//    {
//        return Task.Run(() =>
//        {
//            try
//            {
//                PrincipalContext context = new PrincipalContext(ContextType.Domain);
//                UserPrincipal principal = new UserPrincipal(context);

//                if (context != null)
//                {
//                    principal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guid.ToString());
//                }

//                return AdUser.CastToAdUser(principal);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error retrieving AD User", ex);
//            }
//        });
//    }

//    public Task<List<AdUser>> GetDomainUsers()
//    {
//        return Task.Run(() =>
//        {
//            PrincipalContext context = new PrincipalContext(ContextType.Domain);
//            UserPrincipal principal = new UserPrincipal(context);
//            principal.UserPrincipalName = "*@*";
//            principal.Enabled = true;
//            PrincipalSearcher searcher = new PrincipalSearcher(principal);

//            var users = searcher
//                .FindAll()
//                .AsQueryable()
//                .Cast<UserPrincipal>()
//                .FilterUsers()
//                .SelectAdUsers()
//                .OrderBy(x => x.Surname)
//                .ToList();

//            return users;
//        });
//    }

//    public Task<List<AdUser>> FindDomainUser(string search)
//    {
//        return Task.Run(() =>
//        {
//            PrincipalContext context = new PrincipalContext(ContextType.Domain);
//            UserPrincipal principal = new UserPrincipal(context);
//            principal.SamapplicationUserName = $"*{search}*";
//            principal.Enabled = true;
//            PrincipalSearcher searcher = new PrincipalSearcher(principal);

//            var users = searcher
//                .FindAll()
//                .AsQueryable()
//                .Cast<UserPrincipal>()
//                .FilterUsers()
//                .SelectAdUsers()
//                .OrderBy(x => x.Surname)
//                .ToList();

//            return users;
//        });
//    }
//}