using System.Linq;
using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class DealsRepository : DbRepository<Deal>
{
    public override IQueryable<Deal> Items => base.Items
        .Include(item => item.Book)
        .Include(item => item.Seller)
        .Include(item => item.Buyer);

    public DealsRepository(BookinistDb db) : base(db)
    {
    }
}