using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class CacheFixture : BaseFixture
    {
        private const string Key = "TheKey";

        public CacheFixture()
        {
            Cache.Clear();
        }

        public override void Dispose()
        {
            cache.Verify();
        }

        [Fact]
        public void Count_Should_Use_InternalCache()
        {
            cache.ExpectGet(c => c.Count).Returns(0).Verifiable();

            Assert.Equal(0, Cache.Count);
        }

        [Fact]
        public void Clear_Should_Use_InternalCache()
        {
            cache.Expect(c => c.Clear()).Verifiable();

            Cache.Clear();
        }

        [Fact]
        public void Contains_Should_Use_InternalCache()
        {
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(true).Verifiable();

            Assert.True(Cache.Contains(Key));
        }

        [Fact]
        public void Get_Should_Use_InternalCache_Get()
        {
            cache.Expect(c => c.Get<object>(It.IsAny<string>())).Returns(null).Verifiable();

            Cache.Get<object>(Key);
        }

        [Fact]
        public void TryGet_Should_Use_InternalCache()
        {
            object dummy;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out dummy)).Returns(false).Verifiable();

            Cache.TryGet(Key, out dummy);
        }

        [Fact]
        public void Set_Should_Use_InternalCache()
        {
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            Cache.Set(Key, new object());
        }

        [Fact]
        public void Set_With_AbsoluteExpiration_Should_Uses_InternalCache()
        {
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTime>())).Verifiable();

            Cache.Set(Key, new object(), SystemTime.Now().AddMinutes(5));
        }

        [Fact]
        public void Set_With_SlidingExpiration_Should_Use_InternalCache()
        {
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>())).Verifiable();

            Cache.Set(Key, new object(), TimeSpan.FromMinutes(5));
        }

        [Fact]
        public void Remove_Should_Use_InternalCache()
        {
            cache.Expect(c => c.Remove(It.IsAny<string>())).Verifiable();

            Cache.Remove(Key);
        }
    }
}