using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Material
    {
        [Key]
        public int Id_Material { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }

        [ForeignKey("Brigade")]
        public int? BrigadeId { get; set; }
        public Brigade Brigade { get; set; }
    }
}
