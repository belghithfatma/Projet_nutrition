namespace app_nutri.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public double CaloriesPerUnit { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}
