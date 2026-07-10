using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract;
using Oil_System.Resource.CategoryDtos;
using Oil_System.Service;

namespace Oil_System.Controllers
{
    [Authorize]
    [ApiController]
    public class CategoriesController : AppControllerBase
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        #endregion

        #region Constructor
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region Methods
        [HttpPost(ApiRoute.Category.GetAllCategories)]
        public async Task<IActionResult> GetAllCategories(SearchCategoriesRequest request)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(request);
            return Ok(categories);
        }

        [HttpPost(ApiRoute.Category.GetCategoryById)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPost(ApiRoute.Category.CreateCategory)]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateCategoryAsync(request);
            return Ok(category);
        }

        [HttpPost(ApiRoute.Category.UpdateCategory)]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateCategoryAsync(request);
            return Ok(category);
        }

        [HttpPost(ApiRoute.Category.DeleteCategory)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryService.DeleteCategoryAsync(id);
            return Ok(category);
        }
        #endregion
    }
}
