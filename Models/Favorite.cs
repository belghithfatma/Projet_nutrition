namespace app_nutri.Models;

public class Favorite
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}
