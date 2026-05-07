using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateMyOrder
    {
        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Тип объекта обязателен")]
        public string ObjectType { get; set; }
        [Required(ErrorMessage = "Площадь обязательна")]
        public double Area { get; set; }
        [Required(ErrorMessage = "Тип уборки обязателен")]
        public string CleaningType { get; set; }
        [Required(ErrorMessage = "Дата выполнения обязательна")]
        public DateTime ScheduledDate { get; set; }
    }
}
