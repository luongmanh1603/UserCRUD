using Microsoft.EntityFrameworkCore;
using UserCRUD;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(opt => opt.UseInMemoryDatabase("ListUser"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/users", async (UserDb db) =>
    await db.Users.ToListAsync());

app.MapGet("/users/role", async (UserDb db) =>
    await db.Users.Where(t => t.Role).ToListAsync());

app.MapGet("/users/{id}", async (int id, UserDb db) =>
    await db.Users.FindAsync(id)

        is User user
        ? Results.Ok(user) : Results.NotFound());

app.MapPost("/users", async (User user, UserDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{user.UserId}", user);
});

app.MapPut("/users/{id}", async (int Userid, User inputUser, UserDb db) =>
{
    var user = await db.Users.FindAsync(Userid)
;
    if (user == null) return Results.NotFound();

    user.Username = inputUser.Username;
    user.Role = inputUser.Role;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/users/{id}", async (int Userid, UserDb db) =>
{
    if (await db.Users.FindAsync(Userid)
 is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.Ok(user);
    }
    return Results.NotFound();
});



app.Run();