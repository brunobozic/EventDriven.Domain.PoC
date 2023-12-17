//using Autofac;

//namespace IdentityService.Application.AutofacModules;

//public class DataAccessModule : Module
//{
//    private readonly string _databaseConnectionString;

//    public DataAccessModule(string databaseConnectionString)
//    {
//        _databaseConnectionString = databaseConnectionString;
//    }

//    protected override void Load(ContainerBuilder builder)
//    {
//        //builder.RegisterType<SqlConnectionFactory>()
//        //    .As<ISqlConnectionFactory>()
//        //    .WithParameter("connectionString", _databaseConnectionString)
//        //    .InstancePerLifetimeScope();

//        //builder.RegisterType<UnitOfWork>()
//        //    .As<IUnitOfWork>()
//        //    .InstancePerLifetimeScope();

//        ////builder.RegisterType<CustomerRepository>()
//        ////    .As<ICustomerRepository>()
//        ////    .InstancePerLifetimeScope();

//        ////builder.RegisterType<ProductRepository>()
//        ////    .As<IProductRepository>()
//        ////    .InstancePerLifetimeScope();

//        ////builder.RegisterType<PaymentRepository>()
//        ////    .As<IPaymentRepository>()
//        ////    .InstancePerLifetimeScope();

//        //builder.RegisterType<StronglyTypedIdValueConverterSelector>()
//        //    .As<IValueConverterSelector>()
//        //    .InstancePerLifetimeScope();

//        //builder
//        //    .Register(c =>
//        //    {
//        //        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//        //        dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);
//        //        dbContextOptionsBuilder
//        //            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

//        //        return new ApplicationDbContext(dbContextOptionsBuilder.Options);
//        //    })
//        //    .AsSelf()
//        //    .As<DbContext>()
//        //    .InstancePerLifetimeScope();
//    }
//}