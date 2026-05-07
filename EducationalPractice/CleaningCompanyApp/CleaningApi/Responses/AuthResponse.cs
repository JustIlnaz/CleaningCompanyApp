using CleaningApi.Models;
using System.Collections.Generic;

namespace CleaningApi.Responses
{
    public class AuthResponse
    {
        public bool Status { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }

    public class RegisterResponse
    {
        public bool Status { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }

    public class UsersResponse
    {
        public bool Status { get; set; }
        public List<User> Users { get; set; }
        public string Error { get; set; }
    }

    public class UserResponse
    {
        public bool Status { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }

    public class ProfileResponse
    {
        public bool Status { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }
}
