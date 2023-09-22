using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetValueStringAsync(string key);
        Task SetValueStringAsync(string key, string value);
        Task DeleteKeyValuePairAsync(string key);
        Task RefreshKeyValuePairAsync(string key);

    }
}
