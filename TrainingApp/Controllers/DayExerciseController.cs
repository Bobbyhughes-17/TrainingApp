using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayExerciseController : ControllerBase
    {
        private readonly IDayExerciseRepository _dayExerciseRepository;

        public DayExerciseController(IDayExerciseRepository dayExerciseRepository)
        {
            _dayExerciseRepository = dayExerciseRepository;
        }

        [HttpGet("getDayIdByDayNumber/{dayNumber}")]
        public ActionResult<int> GetDayIdByDayNumber(int dayNumber)
        {
            return _dayExerciseRepository.GetDayIdByDayNumber(dayNumber);
        }

        [HttpGet("getExercisesForUserDay/{userId}/{dayNumber}")]
        public ActionResult<List<Exercise>> GetExercisesForUserDay(int userId, int dayNumber)
        {
            return _dayExerciseRepository.GetExercisesByUserIdAndDayNumber(userId, dayNumber);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dayExerciseRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dayExercise = _dayExerciseRepository.GetById(id);
            if (dayExercise == null)
            {
                return NotFound();
            }
            return Ok(dayExercise);
        }

        [HttpPost]
        public IActionResult Post(DayExercise dayExercise)
        {
            _dayExerciseRepository.Add(dayExercise);
            return CreatedAtAction(nameof(GetById), new { id = dayExercise.Id }, dayExercise);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, DayExercise dayExercise)
        {
            if (id != dayExercise.Id)
            {
                return BadRequest();
            }

            _dayExerciseRepository.Update(dayExercise);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _dayExerciseRepository.Delete(id);
            return NoContent();
        }
        [HttpGet("day/{dayId}")]
        public IActionResult GetByDayId(int dayId)
        {
            return Ok(_dayExerciseRepository.GetByDayId(dayId));
        }

    }
}
