using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Models.Responses;
using Services.PasswordHashers;
using Services.TokenGenerator;
using Services.TokenValidator;
using Services.UserRepositories;
using Services.RefreshTokenRepositories;
using Services.Authenticators;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RefreshTokenValidator _refreshTokenValidator;

        public AuthenticationController(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, Authenticator authenticator, IPasswordHasher passwordHasher, RefreshTokenValidator refreshTokenValidator)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
            _passwordHasher = passwordHasher;
            _refreshTokenValidator = refreshTokenValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            if (registerRequest.password != registerRequest.confirmPassword)
            {
                return BadRequest(new ErrorResponse("Password doesn't match with confirmation"));
            }
            User existingUserByEmail = await _userRepository.getByEmail(registerRequest.email);
            if (existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists"));
            }
            User existingUserByUsername = await _userRepository.getByUsername(registerRequest.username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists"));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.password);

            User registrationUser = new User()
            {
                email = registerRequest.email,
                username = registerRequest.username,
                passwordHash = passwordHash
            };

            await _userRepository.createUser(registrationUser);

            return Ok(registrationUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userRepository.getAll());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest logInRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User existingUserByUsername = await _userRepository.getByUsername(logInRequest.username);
            if (existingUserByUsername == null)
            {
                return Unauthorized();
            }

            bool passwordMatch = _passwordHasher.VerifyPassword(logInRequest.password, existingUserByUsername.passwordHash);
            if (!passwordMatch)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponse response = await _authenticator.authenticate(existingUserByUsername);
            return Ok(response);

        }

        [HttpPut("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValid = _refreshTokenValidator.validate(refreshRequest.refreshToken);
            if (isValid)
            {
                return BadRequest(new ErrorResponse("Invalid Refresh token."));
            }
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.getByToken(refreshRequest.refreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid Refresh token."));

            }
            User userById = await _userRepository.getById(refreshTokenDTO.userId);
            if (userById == null)
            {
                return NotFound(new ErrorResponse("User not found."));
            }
            AuthenticatedUserResponse response = await _authenticator.authenticate(userById);
            return Ok(response);

        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}