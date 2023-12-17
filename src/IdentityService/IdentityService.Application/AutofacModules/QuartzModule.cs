//using System.Reflection;
//using Autofac;
//using Quartz;
//using Module = Autofac.Module;

//namespace IdentityService.Application.AutofacModules;

//public class QuartzModule : Module
//{
//    protected override void Load(ContainerBuilder builder)
//    {
//        var executingAssembly = Assembly.GetExecutingAssembly();
//        builder.RegisterAssemblyTypes(executingAssembly)
//            .Where(x => typeof(IJob).IsAssignableFrom(x)).InstancePerDependency();
//    }
//}