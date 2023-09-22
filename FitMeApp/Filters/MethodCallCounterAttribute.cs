using System;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Filters
{
    public class MethodCallCounterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ICacheService _cache;
        private readonly ILogger<MethodCallCounterAttribute> _logger;

        public MethodCallCounterAttribute(ICacheService cache, ILogger<MethodCallCounterAttribute> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int counter = 1;
            string methodName = context.ActionDescriptor.DisplayName;
            var value = await _cache.GetValueStringAsync(methodName);
            if (value != null)
            {
                counter = Convert.ToInt32(value) + 1;
            }
            
            _logger.LogInformation($"{methodName} - {counter}");
            await _cache.SetValueStringAsync(methodName, $"{counter}");
            await next();
        }
    }
}
