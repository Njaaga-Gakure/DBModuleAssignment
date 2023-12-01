using BookStoreRemastered.models;
using BookStoreSequel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRemastered.services.IServices
{
    public interface IOrders
    {
        Task<bool> createOrder(AddOrder newOrder);
        Task<List<Order>> GetAllOrders();   
    }
}
