using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Application.DTOs.AllItemDtos;
namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddItemController : ControllerBase
    {
       private readonly IAddItemService _addItemService;
        public AddItemController(IAddItemService addItemService)
        {
            _addItemService = addItemService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAddItems(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] string? size,
            [FromQuery] string? sortBy,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 8)
        {
            var pagedResult = await _addItemService.GetPagedItemsAsync(search, categoryId, size, sortBy, page, pageSize);
            return Ok(pagedResult);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddItemById(int id)
        {
            var addItem = await _addItemService.GetAddItemByIdAsync(id);
            if (addItem == null) return NotFound();
            return Ok(addItem);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddItem([FromBody] AddItemDto addItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdAddItem = await _addItemService.CreateAddItemAsync(addItemDto);
            return Ok(createdAddItem);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddItem(int id, [FromBody] UpdateItemDto updateItemDto)
        {
            var updatedAddItem = await _addItemService.UpdateAddItemAsync(id, updateItemDto);
            if (updatedAddItem == null) return NotFound();
            return Ok(updatedAddItem);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddItem(int id)
        {
            var result = await _addItemService.DeleteAddItemAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
