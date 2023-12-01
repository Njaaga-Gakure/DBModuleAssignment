using BookStoreRemastered.models;
using BookStoreRemastered.services;


namespace BookStoreRemastered.controller
{
    public class AuthenticatedUserController
    {
        BooksService bookService = new BooksService();
        OrdersController ordersController = new OrdersController(); 
        public async Task Home(User authenticatedUser) {
            int count = 0;
            do {
                if (count == 0) Console.Clear();
                Console.WriteLine($"Welcome, {authenticatedUser.UserName} - ({authenticatedUser.Role})\n");
                Console.WriteLine("All Books");
                Console.WriteLine("---------\n");
                int bookCount = await PrintAllBooks();
                Console.WriteLine("\n");
                if (authenticatedUser.Role == "admin") {
                    Console.WriteLine("All Orders");
                    Console.WriteLine("---------\n");
                    await ordersController.PrintALLOrders();
                    Console.WriteLine("\n");
                }
                string prompt = authenticatedUser.Role == "admin" ? "Press `c` if you wish to create a book" : bookCount== 0 ? "No book available for purchase" : "Press `b` if you want to buy a book"; 
                Console.WriteLine(prompt);
                Console.WriteLine("press `l` to logout"); 
                string? option = Console.ReadLine();
                if (option == "l") {
                    Console.WriteLine("Logging out...");
                    Thread.Sleep(2000);
                    break;
                }
                var validOptions = new List<string>() { "c", "b"};
                bool isValidOption = ValidateOptions(option, validOptions);
                if (!isValidOption) {
                    Console.Clear();
                    Console.WriteLine("-------- Invalid choice. Please Try Again. -------");
                    count++;
                    continue;
                }
                if (option == "b" && authenticatedUser.Role == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("---------- Admin cannot buy a book. Enter a diffent option. ---------\n");
                    count++;
                    continue;
                }
                else if (option == "c" && authenticatedUser.Role == "user") {
                    Console.Clear();
                    Console.WriteLine("---------- User is not authorized to perform this task. Enter a diffent option. --------\n");
                    count++;
                    continue;
                }
                await redirectUser(option, authenticatedUser.Id);
            } while (true);
        }

        public async Task<int> PrintAllBooks() { 
            var books = await bookService.GetAllBooks();
            int booksCount = books.Count();
            if (booksCount == 0) { 
              Console.WriteLine("No Book Currently in Stock");
                return booksCount;  
            }
            foreach (var book in books) {
                Console.WriteLine($"{book.Id}): {book.BookName} -> {book.Description}");
            }
            return booksCount;
        }
        public bool ValidateOptions(string option, List<string> validOptions) { 
            return validOptions.Contains(option);
        }
        public async Task redirectUser(string option, int userId) {
            switch (option) {
                case "b":
                    await PurchaseBook(userId);
                    break;

                case "c":
                    await AddNewBook();
                    break;
            }
        }

        public async Task AddNewBook() {
            int count = 0;
            do {
                if (count == 0) {
                    Console.Clear();
                }
                Console.WriteLine("Create Book");
                Console.WriteLine("-----------\n");
                Console.WriteLine("Enter a Book Name");
                string? bookName = Console.ReadLine();
                Console.WriteLine("Enter a Book Description");
                string? bookDescription = Console.ReadLine();
                try
                {
                    checkEmptyOrNullValues(new List<string> { bookName, bookDescription });
                }
                catch (Exception ex) {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    count++;
                    continue;
                }
                AddBook newBook = new AddBook() { BookName=bookName, Description=bookDescription};
                bool isSuccess = await bookService.CreateBook(newBook);
                Console.WriteLine(isSuccess ? "--------- Book added successfully ------\n": "--------- Operation Failed. Try Again later ------\n");
                break;
            } while (true);
        }
        public void checkEmptyOrNullValues(List<string> values) {
            foreach (string value in values) {
                if (value == null || value == "") {
                    throw new Exception("\t\t----- Please Fill in all the Fields ------");
                }
            }
        }

        public async Task PurchaseBook(int userId) {
            int count = 0;
            do {
                if (count == 0) Console.Clear();
                Console.WriteLine("Enter The Id of the Book You Wish To buy: ");
                string? option = Console.ReadLine();
                try {
                    await CheckBookIdValidity(option); 
                
                } catch (Exception ex) {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    count++;
                    continue;
                }
                int bookId = int.Parse(option);
                await ordersController.AddBookOrder(userId, bookId);
                Thread.Sleep(2000);
                break;
            } while (true);
        }

        public async Task CheckBookIdValidity(string input) { 
            var books = await bookService.GetAllBooks();
            int booksCount = books.Count();
            bool isInteger = int.TryParse(input, out int number);
            if (!isInteger) {
                throw new Exception("\t\t--------- The Id You Entered Is Invalid. Please Try Again ----------\n");
            }
            if (number < 1 || number > booksCount) {
                throw new Exception("\t\t--------- The Id You Entered Is Invalid. Please Try Again ----------\n");  
            }
        }
    }
}
