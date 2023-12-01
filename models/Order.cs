using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreSequel.models
{
    public class Order
    {
       public int Id { get; set; }  
       public int UserId { get; set; }
       public int BookId { get; set; }
    }
}
