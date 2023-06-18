namespace Backend.Contracts;


public class CreateBookmarkRequest
{

    public string? Link { get; set; }
    public long UserId { get; set; }
    public string? ImgUrl { get; set; }
};

public class BookmarkResponse
{
    public long Id { get; set; }
    public string? Link { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImgUrl { get; set; }
    public long UserId { get; set; }

}
 