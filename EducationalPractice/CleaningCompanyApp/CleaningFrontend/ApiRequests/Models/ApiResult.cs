using CleaningFrontend.ApiRequests.Model;
using System.Collections.Generic;

namespace CleaningFrontend.ApiRequests.Models
{
    public class OrdersResult
    {
        public bool status { get; set; }
        public List<OrderModel> Orders { get; set; }
        public string error { get; set; }
    }

    public class ChecklistResult
    {
        public bool status { get; set; }
        public List<ChecklistModel> ChecklistItems { get; set; }
        public string error { get; set; }
    }

    public class ActionResult
    {
        public bool status { get; set; }
        public string error { get; set; }
    }
}