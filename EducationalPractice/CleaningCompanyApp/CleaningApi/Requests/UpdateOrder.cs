using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class UpdateOrder
    {
        [Required(ErrorMessage = "Id заказа обязателен")]
        public int Id_Order { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? Status { get; set; }
        public string? CleaningType { get; set; }
        public double? Price { get; set; }
        public string? PaymentStatus { get; set; }
        public int? BrigadeId { get; set; }
    }
}
