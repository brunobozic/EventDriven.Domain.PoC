using Autofac;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public static class CompositionRoot
    {
        private static IContainer _container;

        public static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }

        public static void SetContainer(IContainer container)
        {
            _container = container;
        }
    }
}