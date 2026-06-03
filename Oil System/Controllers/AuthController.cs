using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract;
using Oil_System.Contract.Request.Authentcation;
using Oil_System.Service;

namespace Oil_System.Controllers
{
    [ApiController]
    public class AuthController : AppControllerBase
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IValidator<ChangeStatusRequest> _changeStatusValidator;
        #endregion

        #region Constructors
        public AuthController(IAuthenticationService authenticationService, IValidator<RegisterRequest> validator, IValidator<ChangeStatusRequest> changeStatusValidator)
        {
            _authenticationService = authenticationService;
            _validator = validator;
            _changeStatusValidator = changeStatusValidator;
        }
        #endregion

        #region Methods

        [HttpPost(ApiRoute.Account.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers(UsersSearch request)
        {
            var result = await _authenticationService.GetUsersAsync(request);
            return GenericResult(result);
        }

        [HttpPost(ApiRoute.Account.GetById)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _authenticationService.GetUserByIdAsync(id);
            return GenericResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoute.Account.Register)]
        public async Task<IActionResult> CreateUser(RegisterRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(err => new
                {
                    PropertyName = err.PropertyName,
                    ErrorMessage = err.ErrorMessage
                }));
            }

            var result = await _authenticationService.CreateAccountAsync(request);
            return GenericResult(result);
        }


        [HttpPost(ApiRoute.Account.Login)]
        public async Task<IActionResult> LoginUser(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return GenericResult(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoute.Account.Update)]
        public async Task<IActionResult> UpdateUser(ChangeStatusRequest request)
        {
            var validationResult = await _changeStatusValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(err => new
                {
                    PropertyName = err.PropertyName,
                    ErrorMessage = err.ErrorMessage
                }));
            }

            var result = await _authenticationService.ChangeStatusAsync(request);
            return GenericResult(result);
        }

        #endregion
    }
}