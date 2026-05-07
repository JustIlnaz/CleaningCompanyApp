namespace CleaningFrontend.ApiRequests.Model
{
    public class UserModel
    {
        public int Id_User { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Role_Id { get; set; }
        public string RoleName { get; set; }
    }
}
