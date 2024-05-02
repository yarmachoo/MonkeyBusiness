using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Movie;
public class MovieViewModel
{
    public long Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
    [Display(Name = "Режиссер")]
    public string Director { get; set; }
    [Display(Name = "Оценка")]
    public string Grade { get; set; }
    [Display(Name = "Тема")]
    public string Theme { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "Заметка")]
    public string Note { get; set; }
}
