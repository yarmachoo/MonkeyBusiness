using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Book;
public class BookViewModel
{
    public long Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
    [Display(Name = "Автор")]
    public string Author { get; set; }
    [Display(Name = "Оценка")]
    public string Grade { get; set; }
    [Display(Name = "Тема")]
    public string Theme { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "Заметка")]
    public string Note { get; set; }
}
