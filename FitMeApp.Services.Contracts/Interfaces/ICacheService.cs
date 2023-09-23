using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetValueStringAsync(string key);
        Task SetValueAsync<T>(string key, T value, TimeSpan? absoluteExpireTime, TimeSpan? slidingExpireTime);
        Task DeleteKeyValuePairAsync(string key);
        Task RefreshKeyValuePairAsync(string key);

    }
}
