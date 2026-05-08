using System;

namespace CleaningFrontend.ApiRequests.Model
{
    public class ChecklistModel
    {
        public int Id_Checklist { get; set; }
        public string Item { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsVerified { get; set; }
        public string SupervisorRemarks { get; set; }
        public int OrderId { get; set; }
    }
}