namespace CartesianExplosion.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int SubIssueId { get; set; }

    public SubIssue SubIssue { get; set; }
}
