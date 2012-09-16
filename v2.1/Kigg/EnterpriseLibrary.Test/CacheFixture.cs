using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class CacheFixture : IDisposable
    {
        private const string Key = "TheKey";

        private readonly Mock<ICacheManager> _cacheManager;
        private readonly Cache _cache;

        public CacheFixture()
        {
            _cacheManager = new Mock<ICacheManager>();
            _cache = new Cache(_cacheManager.Object);
        }

        public void Dispose()
        {
            _cacheManager.Verify();
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_When_Config_File_Is_Missing()
        {
            Assert.Throws<ConfigurationErrorsException>(() => new Cache("foo"));
        }

        [Fact]
        public void Count_Should_Use_CacheManager()
        {
            _cacheManager.ExpectGet(c => c.Count).Returns(0).Verifiable();

            Assert.Equal(0, _cache.Count);
        }

        [Fact]
        public void Clear_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Flush()).Verifiable();

            _cache.Clear();
        }

        [Fact]
        public void Contains_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Contains(It.IsAny<string>())).Returns(true).Verifiable();

            Assert.True(_cache.Contains(Key));
        }

        [Fact]
        public void Get_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.GetData(It.IsAny<string>())).Returns(new object()).Verifiable();

            _cache.Get<object>(Key);
        }

        [Fact]
        public void TryGet_Should_Use_CacheManager_When_Key_Exists()
        {
            _cacheManager.Expect(c => c.Contains(It.IsAny<string>())).Returns(true).Verifiable();
            _cacheManager.Expect(c => c.GetData(It.IsAny<string>())).Returns(1).Verifiable();

            int dummy;

            _cache.TryGet(Key, out dummy);
        }

        [Fact]
        public void TryGet_Should_Return_Default_Value_When_Key_Does_Not_Exist()
        {
            _cacheManager.Expect(c => c.Contains(It.IsAny<string>())).Returns(false).Verifiable();

            int dummy;

            _cache.TryGet(Key, out dummy);

            Assert.Equal(0, dummy);
        }

        [Fact]
        public void Set_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Add(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            _cache.Set(Key, new object());
        }

        [Fact]
        public void Set_With_AbsoluteExpiration_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Add(It.IsAny<string>(), It.IsAny<object>(), CacheItemPriority.Normal, null, It.IsAny<SlidingTime>()));

            _cache.Set(Key, new object(), SystemTime.Now().AddHours(4));
        }

        [Fact]
        public void Set_With_SlidingExpiration_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Add(It.IsAny<string>(), It.IsAny<object>(), CacheItemPriority.Normal, null, It.IsAny<AbsoluteTime>()));

            _cache.Set(Key, new object(), TimeSpan.FromHours(4));
        }

        [Fact]
        public void RemoveIfExists_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Contains(It.IsAny<string>())).Returns(true).Verifiable();
            _cacheManager.Expect(c => c.Remove(It.IsAny<string>())).Verifiable();

            _cache.RemoveIfExists(Key);
        }

        [Fact]
        public void Remove_Should_Use_CacheManager()
        {
            _cacheManager.Expect(c => c.Remove(It.IsAny<string>())).Verifiable();

            _cache.Remove(Key);
        }
    }
}