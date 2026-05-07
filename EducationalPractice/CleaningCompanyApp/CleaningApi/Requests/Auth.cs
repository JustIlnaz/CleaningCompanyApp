using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class Auth
    {
        [Required(ErrorMessage = "Email обязателен")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }
}
