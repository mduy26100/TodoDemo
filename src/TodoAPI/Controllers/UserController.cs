using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities.ApplicationIdentity;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var userExit = await _userManager.FindByEmailAsync(model.Email);

            if(userExit != null)
            {
                return BadRequest("User already exit");
            }

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors.ToString());
            }

            await _userManager.AddToRoleAsync(user, RoleStatus.User.ToString());

            return Ok(result);
        }
    }
}
