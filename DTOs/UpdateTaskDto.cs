using System;

namespace TeamTaskAPI.DTOs
{
    public record UpdateTaskDto(string Title, string? Description, DateTime DueDate, Guid AssignedToUserId);
}
