using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;
using BookHub.Infrastructure.Repositories;
using BookHub.Infrastructure.Services;
using BookHub.Infrastructure.Services.Auth;

namespace BookHub.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBookHubServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IFavoriteBookService, FavoriteBookService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}