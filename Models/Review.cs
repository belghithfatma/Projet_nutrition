namespace app_nutri.Models;

public class Review
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public string UserName { get; set; } = "Guest";
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
