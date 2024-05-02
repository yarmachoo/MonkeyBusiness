using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.Enum;
public enum Priority
{
    [Display(Name = "На скорую руку")]
    Easy = 1,
    [Display(Name = "Подождет")]
    Medium = 2,
    [Display(Name = "Критично важно")]
    Hard = 3
}
