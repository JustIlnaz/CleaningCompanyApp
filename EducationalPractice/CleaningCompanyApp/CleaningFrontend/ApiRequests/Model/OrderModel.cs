using System;

namespace CleaningFrontend.ApiRequests.Model
{
    public class OrderModel
    {
        public int Id_Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public string CleaningType { get; set; }
        public double Price { get; set; }
        public string PaymentStatus { get; set; }
        public int ObjectId { get; set; }
        public string ObjectAddress { get; set; }
        public int? BrigadeId { get; set; }
        public string BrigadeName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
