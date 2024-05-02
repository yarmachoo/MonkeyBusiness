using System.ComponentModel.DataAnnotations;

namespace MonkeyBuisness.Models.ViewModels.Person;
public class LoginModel
{
    [Required(ErrorMessage = "Не указан логин!")]
    public string Login { get; set; }
    [DataType(DataType.Password)]

    [Required(ErrorMessage = "Не указан пароль!")]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
