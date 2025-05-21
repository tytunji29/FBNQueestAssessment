using System;

namespace FBNQ.DTOs
{
    public record CreateTaskDto(string Title, string? Description, DateTime DueDate, Guid AssignedToUserId);
}
