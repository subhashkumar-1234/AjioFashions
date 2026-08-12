using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Domain.Entities;
using Ecom.Application.DTOs.AllItemDtos;
using Ecom.Application.Interfaces.AllIteam;
namespace Ecom.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            });
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return null;
            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
        }
        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName
            };
            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            return new CategoryDto
            {
                Id = createdCategory.Id,
                CategoryName = createdCategory.CategoryName
            };
        }
        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryUpdateDto CategoryUpdateDto)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null) return null;
            existingCategory.CategoryName = CategoryUpdateDto.CategoryName;
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
            return new CategoryDto
            {
                Id = updatedCategory.Id,
                CategoryName = updatedCategory.CategoryName
            };
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
