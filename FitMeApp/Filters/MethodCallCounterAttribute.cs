﻿using System;
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
            try
            {
                var value = await _cache.GetValueAsync<string>(methodName);
                if (value != null)
                {
                    counter = Convert.ToInt32(value) + 1;
                }

                _logger.LogInformation($"{methodName} - {counter}");
                await _cache.SetValueAsync(methodName, $"{counter}", TimeSpan.FromHours(1), null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            await next();
        }
    }
}
