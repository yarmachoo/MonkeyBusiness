using MonkeyBuisness.Models.Enum;

namespace MonkeyBuisness.Models.ViewModels.Note;
public class CreateNoteViewModel
{
    public string Name { get; set; }
    public string Note { get; set; }
    public string Subject { get; set; }
    public string Theme { get; set; }
    public string Id { get; set; } = "0";

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(Name, "Укажите название конспекта");
        }
        if (string.IsNullOrWhiteSpace(Note))
        {
            throw new ArgumentNullException(Note, "Введите конспект");
        }
        //if (string.IsNullOrWhiteSpace(Subject))
        //{
        //    throw new ArgumentNullException(Subject, "Введите предмет, по которому ведете конспект");
        //}
        if (string.IsNullOrWhiteSpace(Theme))
        {
            throw new ArgumentNullException(Theme, "Введите тему конспекта");
        }
    }
}

