namespace Kigg.Infrastructure
{
    using System.Diagnostics;

    public static class UnitOfWork
    {
        [DebuggerStepThrough]
        public static IUnitOfWork Get()
        {
            return IoC.Resolve<IUnitOfWork>();
        }
    }
}