using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FBNQ.DTOs;
using FBNQ.Services.Interfaces;

namespace FBNQ.Controllers
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
        /// <summary>
        /// To Get All Task Asigned To A Team
        /// </summary>
       
        /// <returns>
        /// 
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetTasks(Guid teamId)
        {
            var tasks = await _taskService.GetTasksAsync(GetUserId(), teamId);
            return Ok(tasks);
        }
        /// <summary>
        /// Create A Task And Asign To Team
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid teamId, CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(GetUserId(), teamId, dto);
            return Ok(task);
        } /// <summary>
          /// Update A Task Asigned To A Team
          /// </summary>
          /// <param name="request">
          /// </param>
          /// <returns>
          /// 
          /// </returns>

        [HttpPut("/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateTaskAsync(GetUserId(), taskId, dto);
            return Ok(task);
        }
        /// <summary>
        /// Delete A Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>

        [HttpDelete("/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            await _taskService.DeleteTaskAsync(GetUserId(), taskId);
            return NoContent();
        }
        /// <summary>
        /// Update A Task Status 
        /// Select Status From 0-2
        /// Call The Get TaskStatus endpoint To Get all status
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPatch("/tasks/{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusDto dto)
        {
            if ((int)dto.Status > 2)
            {
                return BadRequest("Invalid status value. It must be between 0 and 2.");
            }

            var task = await _taskService.UpdateTaskStatusAsync(GetUserId(), taskId, dto);
            return Ok(task);
        }
    }
}
