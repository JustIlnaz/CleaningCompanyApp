namespace CleaningFrontend.ApiRequests.Model
{
    public class ObjectModel
    {
        public int Id_Object { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public double Area { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
    }

    public class CreateMyOrderModel
    {
        public string Address { get; set; }
        public string ObjectType { get; set; } = "Квартира";
        public double Area { get; set; }
        public string CleaningType { get; set; } = "Обычная";
        public System.DateTime ScheduledDate { get; set; } = System.DateTime.Today.AddDays(1);
    }
}
