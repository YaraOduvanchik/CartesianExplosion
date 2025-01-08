namespace CartesianExplosion.Entities;

public class Issue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }

    public Project Project { get; set; }
    public ICollection<SubIssue> SubIssues { get; set; } = new List<SubIssue>();
}
