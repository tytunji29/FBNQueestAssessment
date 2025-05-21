using System;

namespace FBNQ.DTOs
{
    public record UpdateTaskDto(string Title, string? Description, DateTime DueDate, Guid AssignedToUserId);
}
