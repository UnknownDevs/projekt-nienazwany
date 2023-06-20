namespace Backend.Contracts;


public class CreateBookmarkRequest
{

    public string? Link { get; set; }
    public string? ImgUrl { get; set; }
};

public class BookmarkResponse
{
    public string Id { get; set; }
    public string? Link { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgUrl { get; set; }
    public string UserId { get; set; }

}
 