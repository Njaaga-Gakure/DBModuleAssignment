using BookStoreRemastered.models;
using BookStoreRemastered.services;

namespace BookStoreRemastered.controller
{
    
    public class OrdersController
    {
        OrdersService orderService = new OrdersService();  
        BooksService booksService = new BooksService();
        UserService userService = new UserService();    
        public async Task AddBookOrder(int userId, int bookId) { 
            AddOrder addOrder = new AddOrder() { UserId=userId, BookId=bookId};
            bool orderIsSuccessful = await orderService.createOrder(addOrder);
            Console.WriteLine(orderIsSuccessful ? "\t\t-------- Order was Successfull. --------\n" : "\t\t----------- Something Went Wrong. Please Try Again Later. --------------");
        }

        public async Task PrintALLOrders() { 
            var orders = await orderService.GetAllOrders();
            var books = await booksService.GetAllBooks(); 
            var users = await userService.GetAllUsers();
            if (orders.Count == 0) {
                Console.WriteLine("No Order Yet. Check Again Later");
                return;
            }
            foreach (var order in orders) {
                var user = users.Find(user => user.Id == order.UserId);
                var book = books.Find(book => book.Id == order.BookId);
                if (user == null || book == null) {
                    continue;
                }
                Console.WriteLine($"{order.Id}) {user.UserName} ordered {book.BookName}");
            }

        }
    }
}
