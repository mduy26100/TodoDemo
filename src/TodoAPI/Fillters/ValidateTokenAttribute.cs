using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace TodoAPI.Fillters
{
    public class ValidateTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            var request = context.HttpContext.Request;

            //Sử dụng AllowAnonymous
            var endpoint = context.HttpContext.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
            if (allowAnonymous)
            {
                await next();
            }

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedObjectResult("Token không hợp lệ hoặc đã hết hạn");
                return;
            }

            var tokenFromRequest = request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var idUser = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUser))
            {
                context.Result = new UnauthorizedObjectResult("Không tìm thấy id từ token");
                return;
            }

            // Lấy service từ DI
            var cache = context.HttpContext.RequestServices.GetService(typeof(IDistributedCache)) as IDistributedCache;
            var tokenInRedis = await cache.GetStringAsync(idUser);

            if (string.IsNullOrEmpty(tokenInRedis) || tokenInRedis != tokenFromRequest)
            {
                context.Result = new UnauthorizedObjectResult("Token không hợp lệ hoặc đã bị thay đổi");
                return;
            }

            await next(); // cho phép tiếp tục nếu hợp lệ
        }
    }
}
