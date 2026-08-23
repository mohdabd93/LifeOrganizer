namespace LifeOrganizer.Application.Features.Meals;

public class MealDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Calories { get; set; }
    public DateOnly Date { get; set; }
}
