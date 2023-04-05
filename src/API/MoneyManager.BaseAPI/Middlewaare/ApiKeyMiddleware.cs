using Microsoft.AspNetCore.Mvc.Controllers;
using MoneyManager.Application.Contracts.Persistence;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MoneyManager.API.Middlewaare
{
    public class ApiKeyMiddleware : IMiddleware
    {
        private readonly IApiKeyRepository _apiKeyRepository;

        public ApiKeyMiddleware(IApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var controllerActionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
            var apiKeyRequiredAttributeOnController = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(ApiKeyRequiredAttribute), true).FirstOrDefault() as ApiKeyRequiredAttribute;
            var apiKeyRequiredAttributeOnAction = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(ApiKeyRequiredAttribute), true).FirstOrDefault() as ApiKeyRequiredAttribute;
            
            if (apiKeyRequiredAttributeOnController != null || apiKeyRequiredAttributeOnAction != null)
            {
                var he = context.Request.Headers;
                if (!context.Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeaderValues))
                //|| apiKeyHeaderValues.FirstOrDefault() == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API key is missing");
                    return;
                }

                var apiKey = apiKeyHeaderValues.First();

                var validApiKey = await _apiKeyRepository.GetApiKey(apiKey);

                if (validApiKey == null)
                {
                    context.User.Claims.Append(new Claim("ApiKey", apiKey));
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid API key");
                    return;
                }
            }

            await next.Invoke(context);
        }
    }
}
