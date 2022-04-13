using System.Linq;
using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class BooksRepository : DbRepository<Book>
{
    public override IQueryable<Book> Items => base.Items.Include(item => item.Category);

    public BooksRepository(BookinistDb db) : base(db)
    {
    }
}