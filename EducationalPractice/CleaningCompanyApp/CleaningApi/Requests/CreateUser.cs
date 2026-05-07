using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateUser
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email обязателен")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Роль обязательна")]
        public int Role_Id { get; set; }
    }
}
