using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class CreateMaterial
    {
        [Required(ErrorMessage = "Название материала обязательно")]
        public string Name { get; set; }
        public string Unit { get; set; }
        [Required(ErrorMessage = "Количество обязательно")]
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public int? BrigadeId { get; set; }
    }
}
