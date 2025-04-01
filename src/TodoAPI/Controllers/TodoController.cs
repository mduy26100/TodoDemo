using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TodoAPI.Fillters;

namespace TodoAPI.Controllers
{
    [Route("v8/api/todos")]
    [ApiController]
    /*[ValidateToken]*/
    /*[Authorize(Roles = "Admin")]
    [TypeFilter(typeof(ValidateTokenFilter))]*/
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        private readonly IDistributedCache _cache;

        public TodoController(ITodoService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        [HttpGet("GetAllTodo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTodo()
        {
            try
            {
                /*if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Token không hợp lệ hoặc đã hết hạn");
                }

                var tokenFromRequest = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                //Lấy id token trong jwt
                var idUser = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value
                            ?? User.FindFirst(ClaimTypes.Name)?.Value;

                if(string.IsNullOrEmpty(idUser))
                {
                    return Unauthorized("Không tìm thấy id từ token");
                }

                var tokenInRedis = await _cache.GetStringAsync(idUser);

                if (string.IsNullOrEmpty(tokenInRedis))
                {
                    return Unauthorized("Không tìm thấy token trong redis hoặc đã hết hạn");
                }

                if(tokenFromRequest != tokenInRedis)
                {
                    return Unauthorized("Token không khớp với trong redis");
                }*/

                var list = await _service.GetAllTodo();
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("CreateTodo")]
        public async Task<IActionResult> CreateTodo(TodoInsertName insert)
        {
            try
            {
                var list = await _service.CreateTodo(insert);
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("UpdateNameTodo")]
        public async Task<IActionResult> UpdateNameTodo(TodoInsert insert)
        {
            try
            {
                var list = await _service.UpdateNameTodo(insert);
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("UpdateActiveTodo")]
        public async Task<IActionResult> UpdateActiveTodo(TodoInsert insert)
        {
            try
            {
                var list = await _service.UpdateActiveTodo(insert);
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("RemoveTodo/{id}")]
        public async Task<IActionResult> RemoveTodo(int id)
        {
            try
            {
                var list = await _service.RemoveTodo(id);
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
