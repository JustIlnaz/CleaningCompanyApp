using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateBrigade
    {
        [Required(ErrorMessage = "Название бригады обязательно")]
        public string Name { get; set; }
        public int Rating { get; set; }
        public int? BrigadierId { get; set; }
    }
}
