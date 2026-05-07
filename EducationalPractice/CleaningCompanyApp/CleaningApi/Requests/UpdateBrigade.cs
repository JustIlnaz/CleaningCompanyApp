using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class UpdateBrigade
    {
        [Required(ErrorMessage = "Id бригады обязателен")]
        public int Id_Brigade { get; set; }
        public string? Name { get; set; }
        public int? Rating { get; set; }
        public int? BrigadierId { get; set; }
    }
}
