using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract;
using Oil_System.Contract.Request.Authentcation;
using Oil_System.Service;

namespace Oil_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidator<RegisterRequest> _validator;
        #endregion

        #region Constructors
        public AuthController(IAuthenticationService authenticationService, IValidator<RegisterRequest> validator)
        {
            _authenticationService = authenticationService;
            _validator = validator;
        }
        #endregion

        #region Methods
        [HttpPost(ApiRoute.Account.Register)]
        public async Task<IActionResult> CreateUser(RegisterRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(err => new
                {
                    PropertyName = err.PropertyName,
                    ErrorMessage = err.ErrorMessage
                }));
            }

            var result = await _authenticationService.CreateAccountAsync(request);
            return Ok(result);
        }


        [HttpPost(ApiRoute.Account.Login)]
        public async Task<IActionResult> LoginUser(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }


        [HttpPost(ApiRoute.Account.Update)]
        public async Task<IActionResult> UpdateUser(ChangeStatusRequest request)
        {
            var result = await _authenticationService.ChangeStatusAsync(request);
            return Ok(result);
        }

        #endregion
    }
}