using System;

using Xunit;

namespace Kigg.Core.Test
{
    public class DisposableResourceFixtur
    {
        private DisposableResourceTestDouble _resource;

        public DisposableResourceFixtur()
        {
            _resource = new DisposableResourceTestDouble();
        }

        [Fact]
        public void Dispose_Should_Call_Protected_Dispose()
        {
            _resource.Dispose();

            Assert.True(_resource.IsDisposed);

            _resource = null;
            GC.Collect();
        }

        private class DisposableResourceTestDouble  : DisposableResource
        {
            public bool IsDisposed;

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }
    }
}