using AutoMapper;
using FluentValidation;
using Oil_System.BaseInterface;
using Oil_System.Contract.BaseResponse;
using Oil_System.Contract.Pagination;
using Oil_System.Models;
using Oil_System.Resource.BrandDtos;

namespace Oil_System.Service
{
    #region Interfaces
    public interface IBrandService
    {
        Task<BaseResponse<PagedResult<BrandDto>>> GetAllBrandsAsync(SearchBrandsRequest request);
        Task<BaseResponse<BrandDto>> GetBrandByIdAsync(Guid id);
        Task<BaseResponse<bool>> CreateBrandAsync(CreateBrandRequest request);
        Task<BaseResponse<bool>> UpdateBrandAsync(UpdateBrandRequest request);
        Task<BaseResponse<bool>> DeleteBrandAsync(Guid id);
    }
    #endregion

    #region Implementations
    public class BrandService : ResponseHandler, IBrandService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBrandRequest> _createBrandValidator;
        private readonly IValidator<UpdateBrandRequest> _updateBrandValidator;
        #endregion

        #region Constructor
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateBrandRequest> createBrandValidator, IValidator<UpdateBrandRequest> updateBrandValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createBrandValidator = createBrandValidator;
            _updateBrandValidator = updateBrandValidator;
        }
        #endregion

        #region Methods
        public async Task<BaseResponse<PagedResult<BrandDto>>> GetAllBrandsAsync(SearchBrandsRequest request)
        {
            request.addBrandFilters();
            var brands = await _unitOfWork.brandRepository.GetAllAsync(request);

            if (brands == null || !brands.Items.Any())
            {
                return BadRequest<PagedResult<BrandDto>>("No brands found.");
            }

            var result = _mapper.Map<PagedResult<BrandDto>>(brands);
            return Success(result);

        }

        public async Task<BaseResponse<BrandDto>> GetBrandByIdAsync(Guid id)
        {
            var brand = await _unitOfWork.brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return BadRequest<BrandDto>("Brand not found.");
            }

            var result = _mapper.Map<BrandDto>(brand);
            return Success(result);
        }

        public async Task<BaseResponse<bool>> CreateBrandAsync(CreateBrandRequest request)
        {
            var validationResult = await _createBrandValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var brandEntity = _mapper.Map<Brand>(request);
            await _unitOfWork.brandRepository.AddAsync(brandEntity);

            var result = await _unitOfWork.CommitChanges();

            if (!result)
            {
                return BadRequest<bool>("Failed to create brand.");
            }

            return Success(result);
        }

        public async Task<BaseResponse<bool>> UpdateBrandAsync(UpdateBrandRequest request)
        {
            var validationResult = await _updateBrandValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var brandEntity = await _unitOfWork.brandRepository.GetByIdAsync(request.Id);
            if (brandEntity == null)
            {
                return BadRequest<bool>("Brand not found");
            }


            _mapper.Map(request, brandEntity);
            await _unitOfWork.brandRepository.UpdateAsync(brandEntity);
            var result = await _unitOfWork.CommitChanges();
            if (!result)
            {
                return BadRequest<bool>("Failed to update brand.");
            }
            return Success(result);
        }

        public async Task<BaseResponse<bool>> DeleteBrandAsync(Guid id)
        {
            var brandEntity = await _unitOfWork.brandRepository.GetByIdAsync(id);
            if (brandEntity == null)
            {
                return BadRequest<bool>("Brand not found.");
            }

            await _unitOfWork.brandRepository.DeleteAsync(brandEntity.Id);
            var result = await _unitOfWork.CommitChanges();
            if (!result)
            {
                return BadRequest<bool>("Failed to delete brand.");
            }
            return Success(result);
        }
        #endregion
    }
    #endregion
}
