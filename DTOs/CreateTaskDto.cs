using System;

namespace TeamTaskAPI.DTOs
{
    public record CreateTaskDto(string Title, string? Description, DateTime DueDate, Guid AssignedToUserId);
}
