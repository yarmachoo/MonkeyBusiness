using MonkeyBuisness.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Note;
public class NoteViewModel
{
    public long Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
    [Display(Name = "Конспект")]
    public string Note { get; set; }
    [Display(Name = "Дата создания")]
    public string Created { get; set; }
    [Display(Name = "Предмет")]
    public string Subject { get; set; }
    [Display(Name = "Тема")]
    public string Theme { get; set; }

}
