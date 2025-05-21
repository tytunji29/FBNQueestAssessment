using FBNQ.DTOs;
using FBNQ.Models;

namespace FBNQ.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ReturnObject> GetTasksAsync(Guid userId, Guid teamId);
        Task<ReturnObject> CreateTaskAsync(Guid userId, Guid teamId, CreateTaskDto dto);
        Task<ReturnObject> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task<ReturnObject> DeleteTaskAsync(Guid userId, Guid taskId);
        Task<ReturnObject> UpdateTaskStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusDto dto);
    }
}
