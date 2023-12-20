using Microsoft.Extensions.DependencyInjection;
using System;
using Bookinist.DAL.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Bookinist.DAL;

namespace Bookinist.Data
{
    internal static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<BookinistDB>(opt =>
            {
                var type = configuration["Type"];
                switch (type)
                {
                    case "MSSQL": opt.UseSqlServer(configuration.GetConnectionString(type));
                        break;
                    case "SQLite": opt.UseSqlite(configuration.GetConnectionString(type));
                        break;
                    case "InMemory": opt.UseInMemoryDatabase("Bookinist.db");
                        break;
                    default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");
                }
            })
            .AddTransient<DBInitializer>()
            .AddRepositoriesInDB()
            ;
    }
}
