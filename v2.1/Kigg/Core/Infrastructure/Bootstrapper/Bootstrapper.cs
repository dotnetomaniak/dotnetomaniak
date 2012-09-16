namespace Kigg.Infrastructure
{
    using System;

    public static class Bootstrapper
    {
        static Bootstrapper()
        {
            try
            {
                IoC.InitializeWith(new DependencyResolverFactory());
            }
            catch (ArgumentException)
            {
                // Config file is Missing
            }
        }

        public static void Run()
        {
            IoC.ResolveAll<IBootstrapperTask>().ForEach(t => t.Execute());
        }
    }
}