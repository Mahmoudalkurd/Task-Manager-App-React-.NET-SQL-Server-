namespace backend.DTOs;

public class AssignTagsDto
{
    public int TaskId { get; set; }
    public List<int> TagIds { get; set; } = new();
}
