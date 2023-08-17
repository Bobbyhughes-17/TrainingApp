using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgramController : ControllerBase
    {
        private readonly IUserProgramRepository _userProgramRepository;

        public UserProgramController(IUserProgramRepository userProgramRepository)
        {
            _userProgramRepository = userProgramRepository;
        }

        [HttpGet("GetByUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            return Ok(_userProgramRepository.GetByUserId(userId));
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userProgramRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userProgram = _userProgramRepository.GetById(id);
            if (userProgram == null)
            {
                return NotFound();
            }
            return Ok(userProgram);
        }

        [HttpPost]
        public IActionResult Create(UserProgram userProgram)
        {
            var id = _userProgramRepository.Add(userProgram);
            userProgram.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, userProgram);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UserProgram userProgram)
        {
            if (id != userProgram.Id)
            {
                return BadRequest();
            }

            _userProgramRepository.Update(userProgram);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userProgramRepository.Delete(id);
            return NoContent();
        }
    }
}
