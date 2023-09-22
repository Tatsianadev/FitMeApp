using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetValueAsStringAsync(string key);
        Task SetKeyValueAsStringAsync(string key, string value);
        Task<IEnumerable<string>> GetAllKeysAsync();
        Task DeleteKeyValuePairAsync(string key);
        Task DeleteAllKeyValuePairsAsync();

    }
}
