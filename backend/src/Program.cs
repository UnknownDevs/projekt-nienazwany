using Backend.Contracts;
using Backend.Model;
using Supabase;



var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.EnableAnnotations();
});

var  supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var  supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");


builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions{
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    }));


var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.MapPost("/bookmark", async (CreateBookmarkRequest request, Supabase.Client client) =>
{
    var bookmark = new Bookmark
    {
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

app.UseHttpsRedirection();

app.Run();
