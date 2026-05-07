using CleaningFrontend.ApiRequests.Model;
using System.Net.Http.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResult> Auth(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/auth", new { Email = email, Password = password });
            return await response.Content.ReadFromJsonAsync<AuthResult>();
        }

        public async Task<RegisterResult> Register(UserModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/register", user);
            return await response.Content.ReadFromJsonAsync<RegisterResult>();
        }

        public async Task<List<UserModel>> GetAllUsers(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<UsersResult>("api/user/getallusers");
            return response?.Users ?? new List<UserModel>();
        }

        public async Task<UserModel> GetProfile(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<ProfileResult>("api/user/getprofile");
            return response?.User;
        }
    }

    public class AuthResult
    {
        public bool status { get; set; }
        public string token { get; set; }
        public UserModel user { get; set; }
        public string error { get; set; }
    }

    public class UsersResult
    {
        public bool status { get; set; }
        public List<UserModel> Users { get; set; }
    }

    public class ProfileResult
    {
        public bool status { get; set; }
        public UserModel User { get; set; }
    }

    public class RegisterResult
    {
        public bool status { get; set; }
        public UserModel user { get; set; }
        public string error { get; set; }
    }
}
