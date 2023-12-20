using Bookinist.DAL.Entityes;
using Bookinist.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.DAL;

public static class RepositoryRegistrator
{
    public static IServiceCollection AddRepositoriesInDB(this IServiceCollection services) => services
        .AddTransient<IRepository<Book>, BooksRepository>()
        .AddTransient<IRepository<Category>, DBRepository<Category>>()
        .AddTransient<IRepository<Seller>, DBRepository<Seller>>()
        .AddTransient<IRepository<Buyer>, DBRepository<Buyer>>()
        .AddTransient<IRepository<Deal>, DealsRepository>()
        ;
}