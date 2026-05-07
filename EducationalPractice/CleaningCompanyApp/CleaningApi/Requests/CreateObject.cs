using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateObject
    {
        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Тип объекта обязателен")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Площадь обязательна")]
        public double Area { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
    }
}
