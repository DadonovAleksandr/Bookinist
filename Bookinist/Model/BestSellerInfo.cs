using Bookinist.DAL.Entityes;

namespace Bookinist.Model
{
    internal class BestSellerInfo
    {
        public Book Book {  get; set; }
        
        public int SellCount { get; set; }
        public decimal SumCost { get; set; }
    }
}
