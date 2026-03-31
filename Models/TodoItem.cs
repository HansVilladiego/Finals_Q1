namespace Finals_Q1.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public string Hash { get; set; } = string.Empty;
    public string PreviousHash { get; set; } = string.Empty;
}