namespace app_nutri.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Déjeuner";
    public string CuisineType { get; set; } = "Tunisienne";
    public string ImageUrl { get; set; } = string.Empty;
    public int Servings { get; set; } = 1;
    public string AuthorName { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
