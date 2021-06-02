using System;

namespace Server
{
    [Serializable]
    public class Structures
    {
        public int IdProduct { get; set; }
        public int IdDeal { get; set; }
        public int Count { get; set; }

        public Structures(int idProduct, int idDeal, int count)
        {
            IdProduct = idProduct;
            IdDeal = idDeal;
            Count = count;
        }
    }
}
