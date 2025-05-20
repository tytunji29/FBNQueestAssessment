using TeamTaskAPI.DTOs;
using TeamTaskAPI.Models;

namespace TeamTaskAPI.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(Guid userId, Guid teamId);
        Task<TaskItem> CreateTaskAsync(Guid userId, Guid teamId, CreateTaskDto dto);
        Task<TaskItem> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task DeleteTaskAsync(Guid userId, Guid taskId);
        Task<TaskItem> UpdateTaskStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusDto dto);
    }
}
