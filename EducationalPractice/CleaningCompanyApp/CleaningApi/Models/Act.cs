using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Act
    {
        [Key]
        public int Id_Act { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public double TotalAmount { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
