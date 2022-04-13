using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.DAL;

public static class RepositoryRegistrator
{
    public static IServiceCollection AddRepositoriesInDb(this IServiceCollection services) => services
        .AddTransient<IRepository<Category>, DbRepository<Category>>()
        .AddTransient<IRepository<Book>, BooksRepository>()
        .AddTransient<IRepository<Buyer>, DbRepository<Buyer>>()
        .AddTransient<IRepository<Seller>, DbRepository<Seller>>()
        .AddTransient<IRepository<Deal>, DealsRepository>()
    ;
}