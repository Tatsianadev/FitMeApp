using System;
using System.Text.Json;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace FitMeApp.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _redis;
        
        public RedisCacheService(IDistributedCache redis)
        {
            _redis = redis;
        }

        
        public async Task<T> GetValueAsync<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = await _redis.GetStringAsync(key);
            if (value is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(value);
        }
        

        public async Task SetValueAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var jsonData = JsonSerializer.Serialize(value);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(5),
                SlidingExpiration = unusedExpireTime
            };

            await _redis.SetStringAsync(key, jsonData, cacheOptions);
        }


        public async Task DeleteKeyValuePairAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            await _redis.RemoveAsync(key);
        }

        public async Task RefreshKeyValuePairAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            await _redis.RefreshAsync(key);
        }

       
    }
}
