using BookStoreRemastered.models;
using BookStoreRemastered.services.IServices;
using BookStoreSequel.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRemastered.services
{
    public class OrdersService : IOrders
    {
        private readonly HttpClient _httpClient;
        private readonly string _URL = "http://localhost:3000/orders";


        public OrdersService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> createOrder(AddOrder newOrder)
        {
            var content = JsonConvert.SerializeObject(newOrder);
            var body = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_URL, body);
            if (response.IsSuccessStatusCode) { 
                return true;    
            }
            return false;

        }

        public async Task<List<Order>> GetAllOrders()
        {
            var response = await _httpClient.GetAsync(_URL);
            var content = await response.Content.ReadAsStringAsync();

            var orders = JsonConvert.DeserializeObject<List<Order>>(content);
            if (response.IsSuccessStatusCode && orders != null) return orders;
            return new List<Order>();
        }
    }
}
