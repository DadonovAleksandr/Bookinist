using Bookinist.DAL.Context;
using Bookinist.DAL.Entityes;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class BooksRepository : DBRepository<Book>
{
    public override IQueryable<Book> Items => base.Items.Include(item => item.Category);

    public BooksRepository(BookinistDB db) : base(db) { }
}
