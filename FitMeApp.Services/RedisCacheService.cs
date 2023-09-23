using System;
using System.Collections.Generic;
using System.Text;
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

        
        public async Task<string> GetValueStringAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = await _redis.GetStringAsync(key);
            return value;
        }
        

        public async Task SetValueAsync<T>(string key, T value, TimeSpan? absoluteExpireTime, TimeSpan? slidingExpireTime)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await _redis.SetStringAsync(key, value.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime,
                SlidingExpiration = slidingExpireTime
            });
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
