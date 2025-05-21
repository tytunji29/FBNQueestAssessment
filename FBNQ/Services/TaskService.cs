using Microsoft.EntityFrameworkCore;
using FBNQ.Data;
using FBNQ.DTOs;
using FBNQ.Models;
using FBNQ.Services.Interfaces;
using FBNQ.Repository;

namespace FBNQ.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskRepository _repo;

        public TaskService(TaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<ReturnObject> GetTasksAsync(Guid userId, Guid teamId)
        {
           var rec = _repo.GetTask(userId, teamId);
            return new ReturnObject(true, "Record Found", rec);
        }

        public async Task<ReturnObject> CreateTaskAsync(Guid userId, Guid teamId, CreateTaskDto dto)
        {
            _repo.AddTask(userId, teamId, dto);
            return new ReturnObject(true, "Record Added", null);
        }

        public async Task<ReturnObject> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto)
        {
            _repo.UpdateTask(userId, taskId, dto);
            return new ReturnObject(true, "Record Updated", null);
        }

        public async Task<ReturnObject> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            _repo.DeleteTask(userId, taskId);
            return new ReturnObject(true, "Record Deleted", null);
        }

        public async Task<ReturnObject> UpdateTaskStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusDto dto)
        {
            _repo.UpdateTaskStatus(userId, taskId, dto);                                       
            return new ReturnObject(true, "Record Updated", null);
        }
    }
}
