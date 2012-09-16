namespace Kigg.Infrastructure
{
    using System.Diagnostics;

    public static class UnitOfWork
    {
        [DebuggerStepThrough]
        public static IUnitOfWork Begin()
        {
            return IoC.Resolve<IUnitOfWork>();
        }
    }
}