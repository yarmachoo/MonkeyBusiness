using MonkeyBuisness.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonkeyBuisness.Models.Entity;
public class TaskEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime Created { get; set; }
    //public DateTime Deadline { get; set; }
    //public bool IsOverdue { get; set; }
    public bool IsDone { get; set; }
    public long PersonId { get; set; }
    public PersonEntity Person { get; set; }
}
