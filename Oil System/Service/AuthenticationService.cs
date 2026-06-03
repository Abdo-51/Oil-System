using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oil_System.Contract.BaseResponse;
using Oil_System.Contract.Pagination;
using Oil_System.Contract.Request.Authentcation;
using Oil_System.Contract.Response.Authentcation;
using Oil_System.Models;
using Oil_System.Resource.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oil_System.Service
{
    #region Interfaces
    public interface IAuthenticationService
    {
        Task<BaseResponse<AppUserDto>> GetUserByIdAsync(Guid Id);
        Task<BaseResponse<PagedResult<AppUserDto>>> GetUsersAsync(UsersSearch request);
        Task<BaseResponse<RegisterResponse>> CreateAccountAsync(RegisterRequest request);
        Task<BaseResponse<bool>> ChangeStatusAsync(ChangeStatusRequest request);
        Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request);
    }
    #endregion

    #region Implementations
    public class AuthenticationService : ResponseHandler, IAuthenticationService
    {

        #region Fields
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        #endregion

        #region Constractors
        public AuthenticationService(IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper)

        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<BaseResponse<RegisterResponse>> CreateAccountAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return BadRequest<RegisterResponse>("Email is already in use.");
            }

            var newUser = _mapper.Map<AppUser>(request);

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var error = result?.Errors?.FirstOrDefault().Description;
                return BadRequest<RegisterResponse>($"Account creation failed because {error}");
            }

            await _userManager.AddToRoleAsync(newUser, request.Role);

            return Created<RegisterResponse>();
        }
        public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {

                if (!user.IsActive)
                    return BadRequest<LoginResponse>("User Is not Active, Please Contact support");

                var roles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["jwt:ExpireMinutes"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                var Accesstoken = new JwtSecurityTokenHandler().WriteToken(token);

                LoginResponse response = new LoginResponse
                {
                    Token = Accesstoken,
                    Email = user.Email,
                    ExpireAt = DateTime.Now.AddMinutes(double.Parse(_configuration["jwt:ExpireMinutes"])),
                    Roles = roles.ToList()
                };

                return Success(response);
            }
            return BadRequest<LoginResponse>("Invalid credentials");
        }
        public async Task<BaseResponse<bool>> ChangeStatusAsync(ChangeStatusRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                return BadRequest<bool>("User not found");
            }
            user.IsActive = request.IsActive;
            user.UpdatedDate = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest<bool>("Failed to update user status");
            }

            return Updated<bool>();
        }
        public async Task<BaseResponse<PagedResult<AppUserDto>>> GetUsersAsync(UsersSearch request)
        {
            request.AddUsersFilter();

            var users = _userManager.Users.AsQueryable();

            //Apply Filtering
            users = users.ApplyFiltering(request.FilterBy);

            //Apply Sorting
            users = users.ApplySorting(request.SortBy, request.SortDirection);

            //Apply Pagination
            users = users.ApplyPagination(request.PageNumber, request.PageSize);

            if (users == null)
            {
                return BadRequest<PagedResult<AppUserDto>>("No resource found");
            }

            var pagedUsers = await users.ToPagedResultAsync(request.PageNumber, request.PageSize);

            var result = _mapper.Map<PagedResult<AppUserDto>>(pagedUsers);

            return Success<PagedResult<AppUserDto>>(result);
        }
        public async Task<BaseResponse<AppUserDto>> GetUserByIdAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                return BadRequest<AppUserDto>("User not found");
            }

            return Success<AppUserDto>(_mapper.Map<AppUserDto>(user));
        }
        #endregion

    }
    #endregion
}
