namespace CartesianExplosion.Entities;

public class ChangeHistory
{
    public int Id { get; set; }
    public string ChangeDescription { get; set; } = string.Empty;
    public DateTime ChangeDate { get; set; }
    public int SubIssueId { get; set; }

    public SubIssue SubIssue { get; set; }
}
