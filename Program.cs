using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")); // Replace with your MySQL connection string
});

var app = builder.Build();


// Retrieving all items
app.MapGet("/items", async (ToDoDbContext dbContext) =>
{
    var items = await dbContext.Items.ToListAsync();
    return Results.Ok(items);
});

// Adding a new item
app.MapPost("/items", async (ToDoDbContext dbContext, Item item) =>
{
    dbContext.Items.Add(item);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/items/{item.Id}", item);
});

// Item update
app.MapPut("/items/{id}", async (ToDoDbContext dbContext, int id) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    await dbContext.SaveChangesAsync();

    return Results.Ok(item);
});

// Deleting a item
app.MapDelete("/items/{id}", async (ToDoDbContext dbContext, int id) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();







