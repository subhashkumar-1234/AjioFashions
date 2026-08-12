using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Application.DTOs.AllItemDtos;
namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddQuantityController : ControllerBase
    {
        private readonly IAddQuantityService _addQuantityService;

        public AddQuantityController(IAddQuantityService addQuantityService)
        {
            _addQuantityService = addQuantityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAddQuantity()
        {
            var addQuantity = await _addQuantityService.GetAllAddQuantitiesAsync();
            return Ok(addQuantity);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddQuantityById(int id)
        {
            var addQuantity = await _addQuantityService.GetAddQuantityByIdAsync(id);
            return Ok(addQuantity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddQuantity([FromBody] AddQuantityDto addQuantityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdAddQuantity = await _addQuantityService.CreateAddQuantityAsync(addQuantityDto);
            return Ok(createdAddQuantity);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddQuantity(int id, [FromBody] UpdateQuantityDto updateQuantityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedAddQuantity = await _addQuantityService.UpdateAddQuantityAsync(id, updateQuantityDto);
            return Ok(updatedAddQuantity);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddQuantity(int id)
        {
            await _addQuantityService.DeleteAddQuantityAsync(id);
            return NoContent();
        }
    }
}
