namespace Sraplc.Models;

public class Todo
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public bool Completed { get; set; }
}