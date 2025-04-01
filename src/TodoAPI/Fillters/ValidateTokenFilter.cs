using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace TodoAPI.Fillters
{
    public class ValidateTokenFilter : IAsyncActionFilter
    {
        private readonly IDistributedCache _cache;

        public ValidateTokenFilter(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            var headers = context.HttpContext.Request.Headers;

            //Sử dụng AllowAnonymous
            var endpoint = context.HttpContext.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
            if (allowAnonymous)
            {
                await next();
            }

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult("Token không hợp lệ");
                return;
            }

            var tokenFromRequest = headers["Authorization"].ToString().Replace("Bearer ", "");

            var idUser = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUser))
            {
                context.Result = new UnauthorizedObjectResult("Không tìm thấy ID trong token");
                return;
            }

            var tokenInRedis = await _cache.GetStringAsync(idUser);

            if (string.IsNullOrEmpty(tokenInRedis) || tokenInRedis != tokenFromRequest)
            {
                context.Result = new UnauthorizedObjectResult("Token sai hoặc đã hết hạn");
                return;
            }

            await next(); // Token hợp lệ, tiếp tục
        }
    }

}
