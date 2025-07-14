using Chrono.Application;
using Chrono.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Chrono.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;

        public TasksController(TaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? expanded)
        {
            bool includeTimes = expanded == "times";
            var result = await _service.GetAllTasksAsync(includeTimes);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            task.Id = Guid.NewGuid();

            if (task.Times != null)
            {
                foreach (var time in task.Times)
                {
                    time.Id = Guid.NewGuid();
                    time.TaskId = task.Id;
                    time.Task = null;
                }
            }
            await _service.CreateAsync(task);
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/times")]
        public async Task<IActionResult> GetTimes(Guid id)
        {
            var times = await _service.GetTimesByTaskIdAsync(id);
            var result = times.Select(time => new
            {
                time.Id,
                time.Description,
                time.BeginDate,
                time.EndDate,
                SpentTime = ((time.EndDate ?? DateTime.UtcNow) - time.BeginDate).TotalHours
            });
            return Ok(result);
        }

        [HttpPost("{id}/times")]
        public async Task<IActionResult> AddTime(Guid id, [FromBody] TimeEntry time)
        {
            time.Id = Guid.NewGuid();
            await _service.AddTimeEntryAsync(id, time);
            return Ok();
        }

        [HttpPut("{taskId}/times/{timeId}")]
        public async Task<IActionResult> UpdateTime(Guid taskId, Guid timeId, [FromBody] TimeEntry time)
        {
            time.Id = timeId;
            time.TaskId = taskId;
            await _service.UpdateTimeEntryAsync(time);
            return Ok();
        }

        [HttpDelete("{taskId}/times/{timeId}")]
        public async Task<IActionResult> DeleteTime(Guid taskId, Guid timeId)
        {
            await _service.DeleteTimeEntryAsync(taskId, timeId);
            return NoContent();
        }
    }
}
