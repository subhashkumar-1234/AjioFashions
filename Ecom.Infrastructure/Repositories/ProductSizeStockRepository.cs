using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductSizeStockRepository : IProductSizeStockRepository
    {
        private readonly AppDbContext _context;

        public ProductSizeStockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSizeStock>> GetSizeStocksByProductIdAsync(int productId)
        {
            return await _context.ProductSizeStocks
                .AsNoTracking()
                .Where(pss => pss.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductSizeStock?> GetSizeStockAsync(int productId, string size)
        {
            return await _context.ProductSizeStocks
                .FirstOrDefaultAsync(pss => pss.ProductId == productId && pss.Size.ToLower() == size.ToLower());
        }

        public async Task<ProductSizeStock> UpdateSizeStockAsync(int productId, string size, int stock)
        {
            var sizeStock = await GetSizeStockAsync(productId, size);
            if (sizeStock == null)
            {
                sizeStock = new ProductSizeStock
                {
                    ProductId = productId,
                    Size = size,
                    Stock = stock
                };
                _context.ProductSizeStocks.Add(sizeStock);
            }
            else
            {
                sizeStock.Stock = stock;
            }

            await _context.SaveChangesAsync();
            return sizeStock;
        }

        public async Task<bool> CheckStockAsync(int productId, string size, int quantity)
        {
            var sizeStock = await GetSizeStockAsync(productId, size);
            return sizeStock != null && sizeStock.Stock >= quantity;
        }

        public async Task<bool> DecrementStockAsync(int productId, string size, int quantity)
        {
            var sizeStock = await GetSizeStockAsync(productId, size);
            if (sizeStock == null || sizeStock.Stock < quantity)
            {
                return false;
            }

            sizeStock.Stock -= quantity;
            
            // Sync overall product stock
            var product = await _context.AddItems.FindAsync(productId);
            if (product != null)
            {
                product.Stock = (product.Stock >= quantity) ? (product.Stock - quantity) : 0;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
