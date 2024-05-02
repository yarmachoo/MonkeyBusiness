namespace MonkeyBuisness.Models.Entity;
public class NoteEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public DateTime Created {  get; set; }
    public string Subject { get; set; }
    public string Theme { get; set; }
    public long PersonId { get; set; }
    public PersonEntity Person { get; set; }
}
