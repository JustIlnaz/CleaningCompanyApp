using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Requests
{
    public class RequestMaterial
    {
        [Required(ErrorMessage = "Id материала обязателен")]
        public int MaterialId { get; set; }
        [Required(ErrorMessage = "Количество обязательно")]
        public int Quantity { get; set; }
    }
}
