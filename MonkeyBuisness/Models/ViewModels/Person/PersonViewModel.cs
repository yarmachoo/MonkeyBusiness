using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Person
{
    public class PersonViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
