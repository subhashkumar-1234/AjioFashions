using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Application.DTOs.AllItemDtos;

namespace Ecom.Application.Interfaces.AllIteam
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        public Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
        public Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto);
        public Task<bool> DeleteCategoryAsync(int id);
    }
}
