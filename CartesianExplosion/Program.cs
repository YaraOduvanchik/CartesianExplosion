using CartesianExplosion.DataBase;
using CartesianExplosion.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // await context.SeedDataBase();
    }
}

app.MapGet("issues/{id:int}", async (int id, ApplicationDbContext context) =>
{
    var issues = await context.Issues
        .Include(i => i.SubIssues)
        .ThenInclude(s => s.Comments)
        .AsNoTracking()
        .Where(i => i.Id == id)
        .ToListAsync();
    
    return issues;
});

app.MapGet("projects/{id:int}", (int id, ApplicationDbContext context) =>
{
    var projects = context.Projects
        .Include(p => p.Issues)
        .ThenInclude(i => i.SubIssues)
        .ThenInclude(s => s.Comments)
        .AsNoTracking()
        .Where(i => i.Id == id)
        .AsSplitQuery()
        .ToList();
    
    return projects;
});

app.UseHttpsRedirection();

app.Run();