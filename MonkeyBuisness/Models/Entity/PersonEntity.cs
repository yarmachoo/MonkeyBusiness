namespace MonkeyBuisness.Models.Entity;
public class PersonEntity
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public ICollection<TaskEntity> Tasks { get; set; }
    public ICollection<NoteEntity> Notes { get; set; }
    public ICollection<BookEntity> Books { get; set; }
    public ICollection<MovieEntity> Movies { get; set; }
}

