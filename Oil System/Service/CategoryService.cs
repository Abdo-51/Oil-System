using AutoMapper;
using FluentValidation;
using Oil_System.BaseInterface;
using Oil_System.Contract.BaseResponse;
using Oil_System.Contract.Pagination;
using Oil_System.Models;
using Oil_System.Resource.CategoryDtos;

namespace Oil_System.Service
{
    #region Interfaces
    public interface ICategoryService
    {
        Task<BaseResponse<PagedResult<CategoryDto>>> GetAllCategoriesAsync(SearchCategoriesRequest request);
        Task<BaseResponse<CategoryDto>> GetCategoryByIdAsync(Guid id);
        Task<BaseResponse<bool>> CreateCategoryAsync(CreateCategoryRequest category);
        Task<BaseResponse<bool>> UpdateCategoryAsync(UpdateCategoryRequest category);
        Task<BaseResponse<bool>> DeleteCategoryAsync(Guid id);
    }
    #endregion

    #region Implementations
    public class CategoryService : ResponseHandler, ICategoryService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryRequest> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator;
        #endregion

        #region Constructor
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateCategoryRequest> createCategoryValidator, IValidator<UpdateCategoryRequest> updateCategoryValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
        }
        #endregion

        #region Methods
        public async Task<BaseResponse<PagedResult<CategoryDto>>> GetAllCategoriesAsync(SearchCategoriesRequest request)
        {
            request.AddCategoryFilter();
            var categories = await _unitOfWork.categoryRepository.GetAllAsync(request);

            if (categories == null || !categories.Items.Any())
            {
                return BadRequest<PagedResult<CategoryDto>>("No categories found.");
            }

            var result = _mapper.Map<PagedResult<CategoryDto>>(categories);
            return Success(result);
        }

        public async Task<BaseResponse<CategoryDto>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return BadRequest<CategoryDto>("Category not found.");
            }

            var result = _mapper.Map<CategoryDto>(category);
            return Success(result);
        }

        public async Task<BaseResponse<bool>> CreateCategoryAsync(CreateCategoryRequest category)
        {
            var validationResult = await _createCategoryValidator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var categoryEntity = _mapper.Map<Category>(category);
            await _unitOfWork.categoryRepository.AddAsync(categoryEntity);

            var result = await _unitOfWork.CommitChanges();

            if (!result)
            {
                return BadRequest<bool>("Failed to create category.");
            }

            return Success(true);
        }

        public async Task<BaseResponse<bool>> UpdateCategoryAsync(UpdateCategoryRequest category)
        {
            var validationResult = await _updateCategoryValidator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var categoryEntity = await _unitOfWork.categoryRepository.GetByIdAsync(category.Id);
            if (categoryEntity == null)
            {
                return BadRequest<bool>("Category not found.");
            }

            _mapper.Map(category, categoryEntity);
            await _unitOfWork.categoryRepository.UpdateAsync(categoryEntity);

            var result = await _unitOfWork.CommitChanges();

            if (!result)
            {
                return BadRequest<bool>("Failed to update category.");
            }

            return Success(true);
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(Guid id)
        {
            var categoryEntity = await _unitOfWork.categoryRepository.GetByIdAsync(id);

            if (categoryEntity == null)
            {
                return BadRequest<bool>("Category not found.");
            }

            await _unitOfWork.categoryRepository.DeleteAsync(categoryEntity.Id);

            var result = await _unitOfWork.CommitChanges();
            if (!result)
            {
                return BadRequest<bool>("Failed to delete category.");
            }

            return Success(true);
        }
        #endregion
    }
    #endregion
}
