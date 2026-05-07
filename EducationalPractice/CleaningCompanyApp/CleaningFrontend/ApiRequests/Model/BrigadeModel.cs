namespace CleaningFrontend.ApiRequests.Model
{
    public class BrigadeModel
    {
        public int Id_Brigade { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int? BrigadierId { get; set; }
        public string BrigadierName { get; set; }
    }
}
