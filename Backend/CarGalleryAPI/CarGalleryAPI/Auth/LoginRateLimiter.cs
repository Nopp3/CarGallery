using Microsoft.Extensions.Caching.Memory;

namespace CarGalleryAPI.Auth
{
    public class LoginRateLimiter
    {
        private const int MaxFailedAttempts = 5;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(10);
        private readonly IMemoryCache _cache;

        public LoginRateLimiter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool IsBlocked(string username)
        {
            var key = GetKey(username);
            return _cache.TryGetValue<int>(key, out var attempts) && attempts >= MaxFailedAttempts;
        }

        public void RegisterFailure(string username)
        {
            var key = GetKey(username);
            var attempts = 0;

            if (_cache.TryGetValue<int>(key, out var currentAttempts))
                attempts = currentAttempts;

            _cache.Set(key, attempts + 1, Window);
        }

        public void Reset(string username)
        {
            _cache.Remove(GetKey(username));
        }

        private static string GetKey(string username)
        {
            return $"login-fail:{username.Trim().ToLowerInvariant()}";
        }
    }
}
