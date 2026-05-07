namespace CleaningFrontend.ApiRequests.Model
{
    public class MaterialModel
    {
        public int Id_Material { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public int? BrigadeId { get; set; }
        public string BrigadeName { get; set; }
    }
}
