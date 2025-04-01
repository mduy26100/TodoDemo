using Application.DTOs.JWT;
using Application.DTOs.Users;
using Application.Interfaces.Services;
using Domain.Entities.ApplicationIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IDistributedCache _cache;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IDistributedCache cache)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _cache = cache;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.PassWord))
            {
                return Unauthorized("Invalid Email or Password");
            }

            var roleUser = await _userManager.GetRolesAsync(user);

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in roleUser)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var accessToken = _tokenService.GenerateToken(authClaim);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExprytime = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            //Lưu vào cache
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            };
            await _cache.SetStringAsync(user.Id.ToString(), accessToken, cacheOptions);

            return Ok(new AuthenticateResult { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if(tokenModel.RefreshToken == null)
            {
                return BadRequest("Invalid Token Request");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == tokenModel.RefreshToken);

            if(user == null || user.RefreshTokenExprytime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid RefreshToken");
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in userRole)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var newAccessToken = _tokenService.GenerateToken(authClaim);
            /*var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExprytime = DateTime.UtcNow.AddMinutes(10);*/

            //Lưu mới vào cache
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            };
            await _cache.SetStringAsync(user.Id.ToString(), newAccessToken, cacheOptions);

            return Ok(new AuthenticateResult { AccessToken = newAccessToken, RefreshToken = user.RefreshToken });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] TokenModel tokenModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == tokenModel.RefreshToken);

            if(user == null)
            {
                return BadRequest("Invalid Token Request");
            }

            user.RefreshToken = null;
            user.RefreshTokenExprytime = null;

            //Xóa cache
            await _cache.RemoveAsync(user.Id.ToString());

            return Ok("Logout Successfully");
        }
    }
}
