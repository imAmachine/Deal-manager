using System;

namespace Server
{
    [Serializable]
    public class Deals
    {
        public int IdDeal { get; set; }
        public double Sum { get; set; }
        public string Stage { get; set; }

        public Deals(int idDeal, double sum, string stage)
        {
            IdDeal = idDeal;
            Sum = sum;
            Stage = stage;
        }
    }
}
