using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRemastered.models
{
    public class Book
    {
        // initializing properties of the Book Class
        public int Id { get; set; } 
        public string BookName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
