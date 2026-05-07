using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Checklist
    {
        [Key]
        public int Id_Checklist { get; set; }
        public string Item { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
