using System;

namespace Server
{
    [Serializable]
    public class Product
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Product()
        {
        }

        public Product(int idProduct, string name, double price)
        {
            IdProduct = idProduct;
            Name = name;
            Price = price;
        }
    }
}
