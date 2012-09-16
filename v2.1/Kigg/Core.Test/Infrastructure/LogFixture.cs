using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class LogFixture : BaseFixture
    {
        private const string Message = "A dummy message";
        private const string FormatString = "{0}";

        public override void Dispose()
        {
            log.Verify();
        }

        [Fact]
        public void Info_Should_Use_InternalLog()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            Log.Info(Message);
        }

        [Fact]
        public void Info_With_Formatting_Should_Use_InternalLog()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            Log.Info(FormatString, Message);
        }

        [Fact]
        public void Warning_Should_Use_InternalLog()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            Log.Warning(Message);
        }

        [Fact]
        public void Warning_With_Formatting_Should_Use_InternalLog()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            Log.Warning(FormatString, Message);
        }

        [Fact]
        public void Error_Should_Use_InternalLog()
        {
            log.Expect(l => l.Error(It.IsAny<string>())).Verifiable();

            Log.Error(Message);
        }

        [Fact]
        public void Error_With_Formatting_Should_Use_InternalLog()
        {
            log.Expect(l => l.Error(It.IsAny<string>())).Verifiable();

            Log.Error(FormatString, Message);
        }

        [Fact]
        public void Exception_Should_Use_InternalLog()
        {
            log.Expect(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            Log.Exception(new Exception());
        }
    }
}