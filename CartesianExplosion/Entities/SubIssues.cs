namespace CartesianExplosion.Entities;

public class SubIssue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int IssueId { get; set; }

    public Issue Issue { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
