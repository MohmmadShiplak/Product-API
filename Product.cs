using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Products_API
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

     
         public decimal Price { get; set; }


    }
}
