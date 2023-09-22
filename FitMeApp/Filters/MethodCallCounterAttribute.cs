using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Filters
{
    public class MethodCallCounterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<MethodCallCounterAttribute> _logger;

        public MethodCallCounterAttribute(IDistributedCache cache, ILogger<MethodCallCounterAttribute> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int counter = 1;
            string methodName = context.ActionDescriptor.DisplayName;
            var value = await _cache.GetStringAsync(methodName);
            if (value != null)
            {
                counter = Convert.ToInt32(value) + 1;
            }
            
            _logger.LogInformation($"{methodName} - {counter}");
            await _cache.SetStringAsync(methodName, $"{counter}", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            });
            await next();
        }
    }
}
