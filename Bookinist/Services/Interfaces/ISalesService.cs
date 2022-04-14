using System.Collections.Generic;
using System.Threading.Tasks;
using Bookinist.DAL.Entities;

namespace Bookinist.Services.Interfaces;

internal interface ISalesService
{
    IEnumerable<Deal> Deals { get; }
    Task<Deal> MakeADeal(string bookName, Seller seller, Buyer buyer, decimal price);
}