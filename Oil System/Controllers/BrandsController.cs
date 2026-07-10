using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract;
using Oil_System.Resource.BrandDtos;
using Oil_System.Service;

namespace Oil_System.Controllers
{
    [Authorize]
    [ApiController]
    public class BrandsController : AppControllerBase
    {
        #region Fields
        private readonly IBrandService _brandService;
        #endregion

        #region Constructor
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        #endregion

        #region Methods
        [HttpPost(ApiRoute.Brand.GetAllBrands)]
        public async Task<IActionResult> GetAllBrands(SearchBrandsRequest request)
        {
            var brands = await _brandService.GetAllBrandsAsync(request);
            return Ok(brands);
        }

        [HttpPost(ApiRoute.Brand.GetBrandById)]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        [HttpPost(ApiRoute.Brand.CreateBrand)]
        public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
        {
            var brand = await _brandService.CreateBrandAsync(request);
            return Ok(brand);
        }


        [HttpPost(ApiRoute.Brand.UpdateBrand)]
        public async Task<IActionResult> UpdateBrand(UpdateBrandRequest request)
        {
            var brand = await _brandService.UpdateBrandAsync(request);
            return Ok(brand);
        }

        [HttpPost(ApiRoute.Brand.DeleteBrand)]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var brand = await _brandService.DeleteBrandAsync(id);
            return Ok(brand);
        }
        #endregion
    }
}
