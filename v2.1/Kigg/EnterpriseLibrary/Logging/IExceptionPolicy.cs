namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;

    public interface IExceptionPolicy
    {
        bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);
    }
}