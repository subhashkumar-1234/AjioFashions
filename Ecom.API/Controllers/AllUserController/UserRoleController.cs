using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Ecom.Application.Services;
namespace Ecom.API.Controllers.AllUserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var userRoles = await _userRoleService.GetAllUserRolesAsync();
            return Ok(userRoles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRoleById(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null) return NotFound();
            return Ok(userRole);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleCreateDTO userRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdUserRole = await _userRoleService.CreateUserRoleAsync(userRoleDto);
            return Ok(createdUserRole);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UserRoleCreateDTO userRoleDto)
        {
            var userRole = await _userRoleService.UpdateUserRoleAsync(id, userRoleDto);
            return Ok(userRole);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var result = await _userRoleService.DeleteUserRoleAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
