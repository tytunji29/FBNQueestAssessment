namespace FBNQ.DTOs
{
    public record ReturnObject(bool Status, string Message, dynamic? Data);
    public record TeamDto(
    Guid Id,
    string Name,
    List<TeamUserDto> TeamUsers,
    List<TeamTaskDto> Tasks
);

    public record TeamUserDto(string UserId, string Email);
    public record TeamTaskDto(string Id, string Title, string Description);
    public record EnumDto(int Id, string Name);



    public record TaskDto(string Id, string Title, string? Description, DateTime DueDate, Models.TaskStatus Status, DateTime CreatedAt, string TeamName, string AssignedTo);
}
