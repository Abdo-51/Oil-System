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
        Task<BaseResponse<bool>> DeleteUserAsync(Guid Id);
        Task<BaseResponse<AppUserDto>> UpdateUserAsync(AppUserDto request);
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
        /// <summary>
        /// Creates a new user account based on the provided registration details. 
        /// It checks for existing users with the same email, creates a new user if none exists, assigns the specified role, 
        /// and returns an appropriate response indicating success or failure of the account creation process.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A response indicating the result of the account creation process.</returns>
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

        /// <summary>
        /// Authenticates a user based on the provided login credentials. It verifies the user's email and password, checks if the user is active,
        /// retrieves the user's roles, generates a JWT token with the appropriate claims, 
        /// and returns a response containing the token and user information if the authentication is successful. 
        /// If the credentials are invalid or the user is not active, it returns an appropriate error response.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>A response indicating the result of the login process.</returns>
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

        /// <summary>
        /// Changes the active status of a user based on the provided user ID and desired status. It retrieves the user from the database,
        /// updates the status, and saves the changes.
        /// </summary>
        /// <param name="request">The request containing the user ID and desired status.</param>
        /// <returns>A response indicating the result of the status change operation.</returns>
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

        /// <summary>
        /// Retrieves a paginated list of users based on the provided search criteria. It applies filtering, sorting, and pagination to the user query,
        /// and returns the resulting list of users along with pagination information. 
        /// </summary>
        /// <param name="request">The request containing the search criteria.</param>
        /// <returns>A response containing the paginated list of users.</returns>
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

            foreach (var item in result.Items)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
                var roles = await _userManager.GetRolesAsync(user!);
                item.Role = roles.FirstOrDefault() ?? string.Empty;
            }

            return Success<PagedResult<AppUserDto>>(result);
        }

        /// <summary>
        /// Retrieves a user's details based on their unique identifier (ID). It queries the user database for a user matching the provided ID,
        /// </summary>
        /// <param name="Id">The unique identifier of the user to retrieve.</param>
        /// <returns>A response containing the user's details.</returns>
        public async Task<BaseResponse<AppUserDto>> GetUserByIdAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());

            var role = await _userManager.GetRolesAsync(user!);

            if (user == null)
            {
                return BadRequest<AppUserDto>("User not found");
            }

            var userDto = _mapper.Map<AppUserDto>(user);
            userDto.Role = role.FirstOrDefault() ?? string.Empty;

            return Success<AppUserDto>(userDto);
        }

        /// <summary>
        /// Deletes a user from the system based on their unique identifier (ID). It retrieves the user from the database, attempts to delete them,
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A response indicating the success or failure of the deletion operation.</returns>
        public async Task<BaseResponse<bool>> DeleteUserAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
                return BadRequest<bool>("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest<bool>("Failed to delete user");

            return Deleted<bool>();
        }

        /// <summary>
        /// Updates a user's details based on the provided user data transfer object (DTO). It retrieves the user from the database,
        /// </summary>
        /// <param name="request">The user data transfer object containing the updated user information.</param>
        /// <returns>A response containing the updated user's details.</returns>
        public async Task<BaseResponse<AppUserDto>> UpdateUserAsync(AppUserDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
                return BadRequest<AppUserDto>("User not found");

            var result = await _userManager.UpdateAsync(_mapper.Map(request, user));

            if (!result.Succeeded)
            {
                var error = result?.Errors?.FirstOrDefault().Description;
                return BadRequest<AppUserDto>($"Failed to update user because {error}");
            }

            return Success<AppUserDto>(_mapper.Map<AppUserDto>(user));
        }
        #endregion

    }
    #endregion
}
