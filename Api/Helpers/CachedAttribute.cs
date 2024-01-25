using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Api.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLive;

        public CachedAttribute(int timeToLive)
        {
            this.timeToLive = timeToLive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cachedService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contenResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contenResult;
                return;
            }

            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.CacheResponseAsync(cacheKey,okObjectResult.Value,TimeSpan.FromSeconds(timeToLive));
            }
            

        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
