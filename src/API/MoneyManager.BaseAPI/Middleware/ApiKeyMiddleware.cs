using Microsoft.AspNetCore.Mvc.Controllers;
using MoneyManager.API.Attributes;
using MoneyManager.Application.Contracts.Persistence;
using System.Security.Claims;

namespace MoneyManager.API.Middleware
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
                if (!context.Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeaderValues))
                //|| apiKeyHeaderValues.FirstOrDefault() == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API key is missing");
                    return;
                }

                var apiKey = await _apiKeyRepository.GetApiKey(apiKeyHeaderValues.First());

                if (apiKey == null || apiKey.Active == false || apiKey.ExpiresAt < DateTime.Now)
                {
                    //context.User.Claims.Append(new Claim("ApiKey", apiKey));
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid API key");
                    return;
                }
            }

            await next.Invoke(context);
        }
    }
}
