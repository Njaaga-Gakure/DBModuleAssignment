using BookStoreRemastered.models;
using BookStoreRemastered.services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRemastered.services
{
    public class BooksService : IBook
    {

        private readonly HttpClient _httpClient;
        private readonly string _URL = "http://localhost:3000/books";


        public BooksService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreateBook(AddBook newBook)
        {

            var content = JsonConvert.SerializeObject(newBook);
            var body = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_URL, body);

            if (response.IsSuccessStatusCode) {
                return true;
            }
            return false;
        }
        public async Task<List<Book>> GetAllBooks()
        {
            var response = await _httpClient.GetAsync(_URL);
            var content = await response.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<List<Book>>(content);
            if (response.IsSuccessStatusCode && books != null) return books;
            return new List<Book>(); 
        }
        public async Task<Book> GetSingleBook(int Id)
        {
            var response = await _httpClient.GetAsync($"{_URL}/{Id}");
            var content = await response.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<Book>(content);
            if (response.IsSuccessStatusCode && book != null) return book;
            return new Book();
        }
        public async Task<bool> UpdateBook(int Id, AddBook updatedBook)
        {
            var content = JsonConvert.SerializeObject(updatedBook);
            var body = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_URL}/{Id}", body);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteBook(int Id)
        {
            var response = await _httpClient.DeleteAsync($"{_URL}/{Id}");
            if (response.IsSuccessStatusCode) return true;
            return false;
        }



    }
}
