using Bookinist.DAL.Entityes;
using Bookinist.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookinist.Infrastructure.DebugServices
{
    internal class DebugBooksRepository : IRepository<Book>
    {
        public DebugBooksRepository()
        {
            var books = Enumerable.Range(1, 100)
                .Select(i => new Book
                {
                    Id = i,
                    Name = $"Книга {i}",
                    //Category = new Category { Id = 0 }

                })
                .ToArray();

            var categories = Enumerable.Range(1, 10)
                .Select(i => new Category 
                {
                    Id = i,
                    Name = $"Категорая {i}"
                })
                .ToArray();

            foreach(var book in books)
            {
                var category = categories[book.Id % categories.Length];
                category.Books.Add(book);
                book.Category = category;
            }
            Items = books.AsQueryable();
        }
        
        public IQueryable<Book> Items { get; }

        IQueryable<Book> IRepository<Book>.Items => throw new NotImplementedException();

        public Book Add(Book item)
        {
            throw new NotImplementedException();
        }

        public Task<Book> AddAsync(Book item, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Book Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetAsync(int id, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public void Updater(Book item)
        {
            throw new NotImplementedException();
        }

        public Task UpdaterAsync(Book item, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
