using Bookinist.DAL.Entities;

namespace Bookinist.Models;

internal class BestsellerInfo
{
    public Book Book { get; set; }

    public int SellCount { get; set; }

    public decimal SumCost { get; set; }
}