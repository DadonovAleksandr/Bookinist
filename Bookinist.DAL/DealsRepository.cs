using Bookinist.DAL.Context;
using Bookinist.DAL.Entityes;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class DealsRepository : DBRepository<Deal>
{
    public override IQueryable<Deal> Items => base.Items
        .Include(item => item.Book)
        .Include(item => item.Seller)
        .Include(item => item.Buyer)
        ;

    public DealsRepository(BookinistDB db) : base(db) { }
}