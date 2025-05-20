using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskAPI.DTOs;
using TeamTaskAPI.Services.Interfaces;

namespace TeamTaskAPI.Controllers
{
    [ApiController]
    [Route("teams/{teamId}/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetTasks(Guid teamId)
        {
            var tasks = await _taskService.GetTasksAsync(GetUserId(), teamId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid teamId, CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(GetUserId(), teamId, dto);
            return Ok(task);
        }

        [HttpPut("/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateTaskAsync(GetUserId(), taskId, dto);
            return Ok(task);
        }

        [HttpDelete("/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            await _taskService.DeleteTaskAsync(GetUserId(), taskId);
            return NoContent();
        }

        [HttpPatch("/tasks/{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusDto dto)
        {
            var task = await _taskService.UpdateTaskStatusAsync(GetUserId(), taskId, dto);
            return Ok(task);
        }
    }
}
