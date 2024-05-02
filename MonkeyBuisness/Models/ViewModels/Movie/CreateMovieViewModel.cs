namespace MonkeyBuisness.Models.ViewModels.Movie;
public class CreateMovieViewModel
{
    public string Name { get; set; }
    public string Director { get; set; }
    public string Grade { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public string Id { get; set; } = "0";

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(Name, "Укажите название книги");
        }
        if (string.IsNullOrWhiteSpace(Director))
        {
            throw new ArgumentNullException(Director, "Укажитеимя режиссера");
        }
        if (string.IsNullOrWhiteSpace(Theme))
        {
            throw new ArgumentNullException(Theme, "Введите категорию фильма");
        }
    }
}
