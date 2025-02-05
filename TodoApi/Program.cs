using Microsoft.EntityFrameworkCore;
using TodoApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql("name=tododb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
// app.MapGet("/", () => "Welcome to the Minimal API!");
// Define the API endpointsy
app.MapGet("/", () => Results.Redirect("/items"));
app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());


app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
    await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound());

app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});
app.MapPut("/items/{id}", async (int id, bool iscomplete, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.IsComplete = iscomplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

