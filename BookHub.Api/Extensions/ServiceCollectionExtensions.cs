using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;
using BookHub.Core.Validators;
using BookHub.Infrastructure.Messaging;
using BookHub.Infrastructure.Repositories;
using BookHub.Infrastructure.Services;
using BookHub.Infrastructure.Services.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BookHub.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBookHubServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IFavoriteBookService, FavoriteBookService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<BookRequestValidator>();

            return services;
        }
    }
}