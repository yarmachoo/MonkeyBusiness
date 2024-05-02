using MonkeyBuisness.Models.Enum;

namespace MonkeyBuisness.Models.ViewModels.Person
{
    public class CreatePersonViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                throw new ArgumentNullException(Login, "Укажите login");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException(Password, "Укажите password");
            }
        }
    }
}
