using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetValueAsync<T>(string key);
        Task SetValueAsync<T>(string key, T value, TimeSpan? absoluteExpireTime, TimeSpan? unusedExpireTime);
        Task DeleteKeyValuePairAsync(string key);
        Task RefreshKeyValuePairAsync(string key);

    }
}
