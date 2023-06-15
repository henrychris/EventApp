using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters;
using Shared;
using UserAPI.Interfaces;
using AutoMapper;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync());
        }

        [RequiredRolesFilter(UserRoleStrings.Admin, UserRoleStrings.User)]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [RequiredRolesFilter(UserRoleStrings.Admin, UserRoleStrings.User)]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest user)
        {
            var response = await _userService.UpdateUserAsync(id, user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
