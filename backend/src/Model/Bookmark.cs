
namespace Backend.Model;
using Postgrest.Attributes;
using Postgrest.Models;

[Postgrest.Attributes.Table("Bookmark")]
public class Bookmark:BaseModel{
    [PrimaryKey("id", false)]
    public string Id { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("imgUrl")]
    public string ImgUrl { get; set; }

    [Column("link")]
    public string Link { get; set; }
    
    [Column("userId")]
    public string UserId { get; set; }

}