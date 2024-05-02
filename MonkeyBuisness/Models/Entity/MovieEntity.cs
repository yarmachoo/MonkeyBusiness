namespace MonkeyBuisness.Models.Entity;
public class MovieEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Director { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public long Grade { get; set; }
    public string Theme { get; set; }
    public long PersonId { get; set; }
    public PersonEntity Person { get; set; }
}
