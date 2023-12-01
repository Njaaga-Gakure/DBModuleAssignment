using BookStoreRemastered.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRemastered.services.IServices
{
    public interface IBook
    {
        Task<bool> CreateBook(AddBook newBook);
        Task<List<Book>> GetAllBooks();
        Task<Book> GetSingleBook(int Id);
        Task<bool> UpdateBook(int Id, AddBook updatedBook);
        Task<bool> DeleteBook(int Id);
    }
}
