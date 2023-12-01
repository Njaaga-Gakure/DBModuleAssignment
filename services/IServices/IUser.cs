using BookStoreRemastered.models;

namespace BookStoreRemastered.services.IServices
{
    public interface IUser
    {
        Task<List<User>> GetAllUsers();
        Task<bool> CreateUser(AddUser newUser);
    }
}
