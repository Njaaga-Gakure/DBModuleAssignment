using BookStoreRemastered.models;
using BookStoreRemastered.services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace BookStoreRemastered.services
{

    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;
        private readonly string _URL = " http://localhost:3000/users";

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> CreateUser(AddUser newUser)
        {
            var content = JsonConvert.SerializeObject(newUser);
            var body = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_URL, body);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync(_URL);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);

            if (response.IsSuccessStatusCode && users != null) return users;
            return new List<User>();
        }
    }
}
