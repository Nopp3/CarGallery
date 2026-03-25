using CarGalleryAPI.Auth;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace CarGalleryAPI.Tests.Auth
{
    public class LoginRateLimiterTests
    {
        [Fact]
        public void IsBlocked_AfterFiveFailures_ReturnsTrue()
        {
            var limiter = CreateLimiter();
            const string username = "user@example.com";

            for (var i = 0; i < 5; i++)
                limiter.RegisterFailure(username);

            var blocked = limiter.IsBlocked(username);

            Assert.True(blocked);
        }

        [Fact]
        public void Reset_AfterFailures_ClearsBlock()
        {
            var limiter = CreateLimiter();
            const string username = "user@example.com";

            for (var i = 0; i < 5; i++)
                limiter.RegisterFailure(username);

            limiter.Reset(username);

            var blocked = limiter.IsBlocked(username);

            Assert.False(blocked);
        }

        private static LoginRateLimiter CreateLimiter()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new LoginRateLimiter(memoryCache);
        }
    }
}
