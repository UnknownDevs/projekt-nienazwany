using Backend.Contracts;
using Backend.Model;
using Supabase;
using Swashbuckle.AspNetCore.Annotations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.EnableAnnotations();
});

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(builder.Configuration["SUPABASE_URL"], builder.Configuration["SUPABASE_KEY"], new SupabaseOptions{
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    }));


var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var test = app.MapGroup("/test"); // example of mapGroup

//this example present how we can add swagger attributes using the `.WithMetadata()` method
test.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
}).WithMetadata(new SwaggerOperationAttribute("summary001", "description001"));

//this example present how we can add swagger attributes using decorators
test.MapGet("/ping", [SwaggerOperation(Summary = "summary attribute", Description = "description attribute")] () => { 
    return "pong";
});

app.MapPost("/bookmark", async (CreateBookmarkRequest request, Supabase.Client client) =>
{
    var bookmark = new Bookmark
    {
        UserId = request.UserId,
        Link = request.Link,
        ImgUrl = request.ImgUrl
    };

    var response = await client.From<Bookmark>().Insert(bookmark);

    var newBookmark = response.Models.First();
    return Results.Ok(newBookmark.Id);

});

app.MapGet("/bookmark/{id}", async (long id, Supabase.Client client) =>
{
    var response = await client.From<Bookmark>().Where(n => n.Id == id).Get();

    var bookmark = response.Models.FirstOrDefault();

    if (bookmark is null)
    {
        return Results.NotFound();
    };

    var bookmarkResponse = new BookmarkResponse
    {
        Id = bookmark.Id,
        Link = bookmark.Link,
        CreatedAt = bookmark.CreatedAt,
        ImgUrl = bookmark.ImgUrl,
        UserId = bookmark.UserId
    };
    return Results.Ok(bookmarkResponse);
});

app.MapDelete("/bookmark/{id}", async (long id, Supabase.Client client) =>
{
    await client.From<Bookmark>().Where(n => n.Id == id).Delete();
    return Results.NoContent();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
