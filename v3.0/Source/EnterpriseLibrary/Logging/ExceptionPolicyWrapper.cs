namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Runtime.CompilerServices;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public class ExceptionPolicyWrapper : IExceptionPolicy
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            return ExceptionPolicy.HandleException(exceptionToHandle, policyName, out exceptionToThrow);
        }
    }
}