using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Ecom.Application.Services;
namespace Ecom.API.Controllers.AllUserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdRole = await _roleService.CreateRoleAsync(roleDto);
            return Ok(createdRole);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleCreateDTO roleDto)
        {
            var role = await _roleService.UpdateRoleAsync(id, roleDto);
            return Ok(role);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

       
    }
}
