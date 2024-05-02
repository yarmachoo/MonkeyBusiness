using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Book;
public class CreateBookViewModel
{
    public string Name { get; set; }
    public string Author { get; set; }
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
        if (string.IsNullOrWhiteSpace(Author))
        {
            throw new ArgumentNullException(Author, "Укажитеимя автора");
        }
        if (string.IsNullOrWhiteSpace(Theme))
        {
            throw new ArgumentNullException(Theme, "Введите категорию фильма");
        }
    }
}

