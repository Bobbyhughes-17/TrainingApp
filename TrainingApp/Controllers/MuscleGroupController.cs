using Microsoft.AspNetCore.Mvc;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuscleGroupController : ControllerBase
    {
        private readonly IMuscleGroupRepository _muscleGroupRepository;

        public MuscleGroupController(IMuscleGroupRepository muscleGroupRepository)
        {
            _muscleGroupRepository = muscleGroupRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_muscleGroupRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var muscleGroup = _muscleGroupRepository.GetById(id);
            if (muscleGroup == null)
            {
                return NotFound();
            }
            return Ok(muscleGroup);
        }

        [HttpPost]
        public IActionResult Post(MuscleGroup muscleGroup)
        {
            _muscleGroupRepository.Add(muscleGroup);
            return CreatedAtAction(nameof(GetById), new { id = muscleGroup.Id }, muscleGroup);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, MuscleGroup muscleGroup)
        {
            if (id != muscleGroup.Id)
            {
                return BadRequest();
            }

            _muscleGroupRepository.Update(muscleGroup);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _muscleGroupRepository.Delete(id);
            return NoContent();
        }
    }
}
