using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetLogController : ControllerBase
    {
        private readonly ISetLogRepository _setLogRepository;

        public SetLogController(ISetLogRepository setLogRepository)
        {
            _setLogRepository = setLogRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_setLogRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var setLog = _setLogRepository.GetById(id);
            if (setLog == null)
            {
                return NotFound();
            }
            return Ok(setLog);
        }
        [HttpGet("user/{userId}/date/{date}")]
        public IActionResult GetByUserIdAndDate(int userId, DateTime date)
        {
            var logs = _setLogRepository.GetByUserIdAndDate(userId, date);
            if (logs != null && logs.Any())
            {
                return Ok(logs);
            }
            return NotFound();
        }

    [HttpPost]
        public IActionResult Create(SetLog setLog)
        {
            var id = _setLogRepository.Add(setLog);
            setLog.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = id }, setLog);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, SetLog setLog)
        {
            if (id != setLog.Id)
            {
                return BadRequest();
            }

            _setLogRepository.Update(setLog);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _setLogRepository.Delete(id);
            return NoContent();
        }
    }
}

