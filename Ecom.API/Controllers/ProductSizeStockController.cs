using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductSizeStockController : ControllerBase
    {
        private readonly IProductSizeStockService _service;

        public ProductSizeStockController(IProductSizeStockService service)
        {
            _service = service;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetSizeStocks(int productId)
        {
            var stocks = await _service.GetSizeStocksByProductIdAsync(productId);
            return Ok(stocks);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSizeStock([FromBody] ProductSizeStockDto dto)
        {
            var updated = await _service.UpdateSizeStockAsync(dto);
            return Ok(updated);
        }
    }
}
