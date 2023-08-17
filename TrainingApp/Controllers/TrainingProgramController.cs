using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public TrainingProgramController(ITrainingProgramRepository trainingProgramRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_trainingProgramRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }
            return Ok(trainingProgram);
        }

        [HttpPost]
        public IActionResult Post(TrainingProgram trainingProgram)
        {
            _trainingProgramRepository.Add(trainingProgram);
            return CreatedAtAction(nameof(GetById), new { id = trainingProgram.Id }, trainingProgram);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.Id)
            {
                return BadRequest();
            }

            _trainingProgramRepository.Update(trainingProgram);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _trainingProgramRepository.Delete(id);
            return NoContent();
        }

    }
}
