using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : ControllerBase
    {
        private readonly IDayRepository _dayRepository;

        public DayController(IDayRepository dayRepository)
        {
            _dayRepository = dayRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dayRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var day = _dayRepository.GetById(id);
            if (day == null)
            {
                return NotFound();
            }
            return Ok(day);
        }

        [HttpPost]
        public IActionResult Post(Day day)
        {
            _dayRepository.Add(day);
            return CreatedAtAction(nameof(GetById), new { id = day.Id }, day);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Day day)
        {
            if (id != day.Id)
            {
                return BadRequest();
            }

            _dayRepository.Update(day);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _dayRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("byTrainingProgram/{trainingProgramId}")]
        public IActionResult GetByTrainingProgram(int trainingProgramId)
        {
            var days = _dayRepository.GetDaysByTrainingProgramId(trainingProgramId);
            if (days == null)
            {
                return NotFound();
            }
            return Ok(days);
        }

    }
}
