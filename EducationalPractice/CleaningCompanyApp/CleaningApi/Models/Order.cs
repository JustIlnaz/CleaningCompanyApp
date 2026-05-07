using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Order
    {
        [Key]
        public int Id_Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public string CleaningType { get; set; }
        public double Price { get; set; }
        public string PaymentStatus { get; set; }

        [ForeignKey("CleaningObject")]
        public int ObjectId { get; set; }
        public CleaningObject CleaningObject { get; set; }

        [ForeignKey("Brigade")]
        public int? BrigadeId { get; set; }
        public Brigade Brigade { get; set; }

        [ForeignKey("User")]
        public int ClientId { get; set; }
        public User Client { get; set; }
    }
}
