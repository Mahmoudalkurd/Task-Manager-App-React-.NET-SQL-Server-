namespace backend.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public bool Completed { get; set; }
    public DateTime? DueDate { get; set; }

    // One User → Many Tasks
    public int UserId { get; set; }
    public User? User { get; set; }

    // Many-to-many → TaskTags
    public List<TaskTag> TaskTags { get; set; } = new();
}

public class TaskTag
{
    public int TaskItemId { get; set; }
    public TaskItem? TaskItem { get; set; }

    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}
public class AssignTagsDto
{
    public int TaskId { get; set; }
    public List<int> TagIds { get; set; } = new();
}
