using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            User user = _userRepository.GetByEmail(loginRequest.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            if (_userRepository.ValidateUser(loginRequest.Email, loginRequest.Password))
            {
                var token = _userRepository.GenerateToken(user);
                return Ok(new { token, userId = user.Id });
            }
            else
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }


    [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserUpdateDTO userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new { message = "Id mismatch" });
            }

            var existingUser = _userRepository.GetByEmail(userDto.Email);
            if (existingUser == null)
            {
                return NotFound();
            }
            User user = new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                MaxBench = userDto.MaxBench,
                MaxSquat = userDto.MaxSquat,
                MaxDeadlift = userDto.MaxDeadlift
            };

            _userRepository.UpdateUser(user);
            return NoContent();
        }


    [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }

    }
}
