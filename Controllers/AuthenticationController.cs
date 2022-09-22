using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using Models.Responses;
using Services.PasswordHashers;
using Services.TokenGenerator;
using Services.UserRepositories;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher, AccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
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
            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.email);
            if (existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists"));
            }
            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.username);
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

            await _userRepository.CreateUser(registrationUser);

            return Ok(registrationUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userRepository.GetAll());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest logInRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User existingUserByUsername = await _userRepository.GetByUsername(logInRequest.username);
            if (existingUserByUsername == null)
            {
                return Unauthorized();
            }

            bool passwordMatch = _passwordHasher.VerifyPassword(logInRequest.password, existingUserByUsername.passwordHash);
            if (!passwordMatch)
            {
                return Unauthorized();
            }

            string accessToken = _accessTokenGenerator.Generate(existingUserByUsername);
            return Ok(new AuthenticatedUserResponse() { accessToken = accessToken });
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}