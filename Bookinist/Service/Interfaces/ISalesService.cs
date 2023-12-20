using Bookinist.DAL.Entityes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookinist.Service.Interfaces;

internal interface ISalesService
{
    IEnumerable<Deal> Deals { get; }

    Task<Deal> MakeDeal(string bookName, Seller seller, Buyer buyer, decimal price);

}