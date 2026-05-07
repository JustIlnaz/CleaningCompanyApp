using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class UpdateUser
    {
        [Required(ErrorMessage = "Id пользователя обязателен")]
        public int Id_User { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public int? Role_Id { get; set; }
    }
}
