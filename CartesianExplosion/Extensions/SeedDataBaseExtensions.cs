using Bogus;
using CartesianExplosion.DataBase;
using CartesianExplosion.Entities;

namespace CartesianExplosion.Extensions;

public static class SeedDataBaseExtensions
{
    public static async Task SeedDataBase(this ApplicationDbContext context)
    {
        Randomizer.Seed = new Random(1000000);

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var projectFaker = new Faker<Project>()
                .RuleFor(p => p.Name, f => f.Commerce.Department())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.StartDate, f => f.Date.Past(1).ToUniversalTime())
                .RuleFor(p => p.EndDate, f => f.Date.Future(1).ToUniversalTime());

            var projects = projectFaker.Generate(15);
            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();

            foreach (var project in projects)
            {
                var issueFaker = new Faker<Issue>()
                    .RuleFor(i => i.Title, f => f.Lorem.Sentence())
                    .RuleFor(i => i.Description, f => f.Lorem.Paragraph())
                    .RuleFor(i => i.CreatedAt, f => f.Date.Past(3).ToUniversalTime())
                    .RuleFor(i => i.DueDate, f => f.Date.Future(1).ToUniversalTime())
                    .RuleFor(i => i.ProjectId, project.Id);

                var issues = issueFaker.Generate(10);
                project.Issues = issues;
                await context.SaveChangesAsync();

                foreach (var issue in issues)
                {
                    var subIssueFaker = new Faker<SubIssue>()
                        .RuleFor(si => si.Title, f => f.Lorem.Sentence())
                        .RuleFor(si => si.Description, f => f.Lorem.Paragraph())
                        .RuleFor(si => si.CreatedAt, f => f.Date.Past(1).ToUniversalTime())
                        .RuleFor(si => si.DueDate, f => f.Date.Future(1).ToUniversalTime())
                        .RuleFor(si => si.IssueId, issue.Id);

                    var subIssues = subIssueFaker.Generate(10);
                    issue.SubIssues = subIssues;
                    await context.SaveChangesAsync();

                    foreach (var subIssue in subIssues)
                    {
                        var commentFaker = new Faker<Comment>()
                            .RuleFor(c => c.Text, f => f.Lorem.Sentence())
                            .RuleFor(c => c.CreatedAt, f => f.Date.Past(1).ToUniversalTime())
                            .RuleFor(c => c.SubIssueId, subIssue.Id);

                        var comments = commentFaker.Generate(3);
                        subIssue.Comments = comments;
                        await context.SaveChangesAsync();

                        var changeHistoriesFaker = new Faker<ChangeHistory>()
                            .RuleFor(c => c.ChangeDescription, f => f.Lorem.Sentence())
                            .RuleFor(c => c.ChangeDate, f => f.Date.Past(1).ToUniversalTime())
                            .RuleFor(c => c.SubIssueId, subIssue.Id);

                        var changeHistories = changeHistoriesFaker.Generate(3);

                        await context.ChangeHistories.AddRangeAsync(changeHistories);
                        await context.SaveChangesAsync();
                    }
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}