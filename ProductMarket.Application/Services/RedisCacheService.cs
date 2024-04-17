using Microsoft.Extensions.Caching.Distributed;
using ProductMarket.Domain.Interfaces.Services;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace ProductMarket.Domain.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T Get<T>(string key)
        {
            var value = _cache.GetString(key);
            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }
        public T Set<T>(string key, T value)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };
            _cache.SetString(key, JsonSerializer.Serialize(value), timeOut);
            return value;
        }
        public T Delete<T>(string key)
        {
            var value = Get<T>(key);
            _cache.Remove(key);
            return value;
        }
        public List<T> GetAllKeys<T>()
        {
            List<string> listKeys = new();
            List<T> resourses = new();
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,allowAdmin=true"))
            {
                var keys = redis.GetServer("localhost", 6379).Keys();
                listKeys.AddRange(keys.Select(key => (string)key).ToList());
            }
            foreach (var key in listKeys)
            {
                resourses.Add(Get<T>(key));
            }
            return resourses;
        }
    }
}
