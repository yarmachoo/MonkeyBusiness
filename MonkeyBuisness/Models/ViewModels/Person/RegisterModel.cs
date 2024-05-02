using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Person;
public class RegisterModel
{ 
    [Required(ErrorMessage="Не указан логин!")]
    public string Login { get; set; }
    [DataType(DataType.Password)]

    [Required(ErrorMessage = "Не указан пароль!")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage ="Пароль введен невероно!")]
    [Display(Name = "Password")]
    public string ConfirmPassword { get; set; }
}
