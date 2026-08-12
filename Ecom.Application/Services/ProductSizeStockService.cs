using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class ProductSizeStockService : IProductSizeStockService
    {
        private readonly IProductSizeStockRepository _repository;

        public ProductSizeStockService(IProductSizeStockRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductSizeStockDto>> GetSizeStocksByProductIdAsync(int productId)
        {
            var stocks = await _repository.GetSizeStocksByProductIdAsync(productId);
            return stocks.Select(s => new ProductSizeStockDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                Size = s.Size,
                Stock = s.Stock
            });
        }

        public async Task<ProductSizeStockDto> UpdateSizeStockAsync(ProductSizeStockDto dto)
        {
            var s = await _repository.UpdateSizeStockAsync(dto.ProductId, dto.Size, dto.Stock);
            return new ProductSizeStockDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                Size = s.Size,
                Stock = s.Stock
            };
        }

        public async Task<bool> CheckStockAsync(int productId, string size, int quantity)
        {
            return await _repository.CheckStockAsync(productId, size, quantity);
        }
    }
}
