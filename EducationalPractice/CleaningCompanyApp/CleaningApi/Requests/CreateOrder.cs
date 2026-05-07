using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateOrder
    {
        [Required(ErrorMessage = "Дата выполнения обязательна")]
        public DateTime ScheduledDate { get; set; }
        [Required(ErrorMessage = "Тип уборки обязателен")]
        public string CleaningType { get; set; }
        [Required(ErrorMessage = "Цена обязательна")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Id объекта обязательно")]
        public int ObjectId { get; set; }
        [Required(ErrorMessage = "Id клиента обязательно")]
        public int ClientId { get; set; }
    }
}
