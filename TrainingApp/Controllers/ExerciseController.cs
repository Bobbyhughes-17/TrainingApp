using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_exerciseRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var exercise = _exerciseRepository.GetById(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        [HttpPost]
        public IActionResult Post(Exercise exercise)
        {
            _exerciseRepository.Add(exercise);
            return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return BadRequest();
            }

            _exerciseRepository.Update(exercise);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _exerciseRepository.Delete(id);
            return NoContent();
        }
        [HttpGet("musclegroup/{muscleGroupId}")]
        public IActionResult GetByMuscleGroupId(int muscleGroupId)
        {
            var exercises = _exerciseRepository.GetByMuscleGroupId(muscleGroupId);
            if (exercises == null || !exercises.Any())
            {
                return NotFound();
            }
            return Ok(exercises);
        }
    }
}
