using BookStoreRemastered.models;
using BookStoreRemastered.services;


namespace BookStoreRemastered.controller
{
    public class AuthController
    {
        UserService userService = new UserService();
        AuthenticatedUserController authUser = new AuthenticatedUserController();

        public async Task init() {
            do
            {
                Console.WriteLine("Press `1` to register.");
                Console.WriteLine("Press `2` to login.");
                Console.WriteLine("Press `q` to quit the application.");
                var option = Console.ReadLine();
                if (option == null) {
                    Console.WriteLine("Invalid Input");
                    continue;
                }
                if (option == "q") {
                    Console.WriteLine("Bye...");
                    break;
                }

                var validOptions = new List<string> { "1", "2" };

                bool isValidOption = validateOptions(option, validOptions);
                if (!isValidOption) {
                    Console.Clear();
                    Console.WriteLine("Invalid Option. Please Try Again... :(");
                    continue;
                }
                await redirectUser(option);
            } while (true);
        }

        public bool validateOptions(string option, List<string> validOptions) {
            return validOptions.Contains(option);
        }
        public async Task redirectUser(string option) {
            switch (option) {
                case "1":
                    await registerUser();
                    break;
                case "2":
                    await loginUser();
                    break;
            }
        }
        public async Task registerUser() {
            int count = 0;
            do
            {
                if (count == 0) Console.Clear();
                Console.WriteLine("\t\tRegistration Form");
                Console.WriteLine("\t\t-----------------\n");
                Console.WriteLine("Enter username:");
                string? username = Console.ReadLine().Trim();
                Console.WriteLine("Enter password:");
                string? password = Console.ReadLine().Trim();

                try { 
                
                    isFieldEmpty(username, password);
                
                } catch (Exception ex) {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    count++;
                    continue;
                }
               
                var isUser = await UserExists(username);
                if (isUser) {
                    Console.Clear();
                    Console.WriteLine($"\t ------------ A username `{username}` already exists. Please enter a unique value. -------------\n");
                    count++;
                    continue;
                }
                await AddNewUser(username, password);
            } while (true);
        
        }

        public async Task loginUser() {
            int count = 0;
            do
            {
                if (count == 0) Console.Clear();
                Console.WriteLine("\t\tLogin Form");
                Console.WriteLine("\t\t-------------\n");
                Console.WriteLine("Enter username:");
                string? username = Console.ReadLine().Trim();
                Console.WriteLine("Enter password:");
                string? password = Console.ReadLine().Trim();
                try
                {
                    isFieldEmpty(username, password);

                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    count++;
                    continue;
                }
                var isUser = await UserExists(username);
                if (!isUser)
                {
                    Console.Clear();
                    Console.WriteLine($"\t ------------ Invalid Credentials. Please Try Again -----------");
                    count++;
                    continue;
                }
                User user = await ValidateLoginUser(username, password);
                if (user.UserName == "") {
                    Console.Clear();
                    Console.WriteLine($"\t ------------ Invalid Credentials. Please Try Again -----------");
                    count++;
                    continue;
                }
                Console.WriteLine("Logging Successful... Redirecting to Home Page... :)");
                Thread.Sleep(2000);
                await authUser.Home(user);
            } while (true);
        }

        public void isFieldEmpty(string username, string password)
        {
            if (username == null || username == "" || password == null || password == "") {
                throw new Exception("\t---- Please fill in all the fields!!! ----");
            };
             
        }
        public async Task<bool> UserExists(string username)
        {
            var users = await userService.GetAllUsers();
            int userCount = users.Where(user => user.UserName == username).Count();

            return userCount > 0;
        }

        public async Task<bool> isFirstUser() {
            var users = await userService.GetAllUsers();
            return users.Count == 0;
        }
        public async Task AddNewUser(string username, string password) {
            string role = "user";
            bool firstUser = await isFirstUser();
            if (firstUser) {
                role = "admin";
            }
            AddUser newUser = new AddUser() { UserName=username, Password=password, Role=role};
            bool isSuccess = await userService.CreateUser(newUser);
            Console.Clear();
            Console.WriteLine(isSuccess ? "\t-------- Registration was successful ---------\n" : "\t--------- Something went wrong. Try again later.-----------\n");
            await init();

        }

        public async Task<User> ValidateLoginUser(string username, string password)
        {
            List<User> users = await userService.GetAllUsers();
            User? user = users.Find(u => u.UserName == username && u.Password == password);
            return user != null ? user : new User();
        }

    }
}
