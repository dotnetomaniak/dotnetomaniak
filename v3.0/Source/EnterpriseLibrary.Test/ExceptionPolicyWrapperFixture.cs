using System;

using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class ExceptionPolicyWrapperFixture
    {
        [Fact]
        public void HandleException_Should_Not_Throw_Exception_When_Config_File_Is_Missing()
        {
            ExceptionPolicyWrapper epw = new ExceptionPolicyWrapper();

            Exception exceptionToThrow;

            Assert.DoesNotThrow(() => epw.HandleException(new Exception(), "foo", out exceptionToThrow));
        }
    }
}