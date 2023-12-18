using Bookinist.DAL.Context;
using Bookinist.DAL.Entityes;
using Bookinist.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinist.Data
{
    internal class DBInitializer
    {
        private readonly BookinistDB _db;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(BookinistDB db, ILogger<DBInitializer> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task InitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            //_logger.LogInformation("Инициализация БД ...");
            //await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);
            //_logger.LogInformation($"Удаление существующей БД выполнена за {timer.ElapsedMilliseconds} мс");
            //_db.Database.EnsureCreated();
            _logger.LogInformation("Миграция БД ...");
            await _db.Database.MigrateAsync();
            _logger.LogInformation($"Миграция БД выполнена за {timer.ElapsedMilliseconds} мс");

            if (await _db.Books.AnyAsync()) return;

            await InitializeCategoriesAsync();
            await InitializeBooksAsync();
            await InitializeBuyersAsync();
            await InitializeSellersAsync();
            await InitializeDealsAsync();

            _logger.LogInformation($"Инициализация БД выполнена за {timer.Elapsed.TotalSeconds} с");

        }

        private const int _categoryCount = 10;
        private Category[] _categories;
        private async Task InitializeCategoriesAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы категорий в БД ...");

            _categories = new Category[_categoryCount];
            for(int i = 0; i < _categoryCount; i++)
            {
                _categories[i] = new Category
                {
                    Name = $"Категория {i+1}"
                };
            }
            _db.Categories.AddRange(_categories);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Инициализация таблицы категорий в БД выполнена за {timer.ElapsedMilliseconds} мс");
        }

        private const int _booksCount = 10;
        private Book[] _books;
        private async Task InitializeBooksAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы книги в БД ...");

            var rnd = new Random();
            _books = Enumerable.Range(1, _booksCount)
                .Select(i => new Book()
                {
                    Name = $"Книга {i}",
                    Category = rnd.NextItem(_categories)
                })
                .ToArray();
            await _db.Books.AddRangeAsync(_books);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Инициализация таблицы книги в БД выполнена за {timer.ElapsedMilliseconds} мс");
        }

        private const int _sellersCount = 10;
        private Seller[] _sellers;
        private async Task InitializeSellersAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы продавцов в БД ...");

            _sellers = Enumerable.Range(1, _sellersCount)
                .Select(i => new Seller()
                {
                    Name = $"Продавец-Имя {i}",
                    Surname = $"Продавец-Фамилия {i}",
                    Patronymic = $"Продавец-Отчество {i}"
                })
                .ToArray();
            await _db.Sellers.AddRangeAsync(_sellers);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Инициализация таблицы продавцов в БД выполнена за {timer.ElapsedMilliseconds} мс");
        }

        private const int _buyersCount = 10;
        private Buyer[] _buyers;
        private async Task InitializeBuyersAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы покупателей в БД ...");

            _buyers = Enumerable.Range(1, _buyersCount)
                .Select(i => new Buyer()
                {
                    Name = $"Покупатель-Имя {i}",
                    Surname = $"Покупатель-Фамилия {i}",
                    Patronymic = $"Покупатель-Отчество {i}"
                })
                .ToArray();
            await _db.Buyers.AddRangeAsync(_buyers);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Инициализация таблицы покупателей в БД выполнена за {timer.ElapsedMilliseconds} мс");
        }

        private const int _dealsCount = 1_000;
        private Deal[] _deals;
        private async Task InitializeDealsAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация таблицы сделок в БД ...");

            var rnd = new Random();
            _deals = Enumerable.Range(1, _dealsCount)
                .Select(i => new Deal()
                {
                    Book = rnd.NextItem(_books),
                    Seller = rnd.NextItem(_sellers),
                    Buyer = rnd.NextItem(_buyers),
                    Price = (decimal) rnd.NextDouble() * 5000 + 700
                })
                .ToArray();
            await _db.Deals.AddRangeAsync(_deals);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Инициализация таблицы сделок в БД выполнена за {timer.ElapsedMilliseconds} мс");
        }

    }
}
