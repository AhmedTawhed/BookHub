using BookHub.Core.Interfaces.Repository;

namespace BookHub.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        ICategoryRepository Categories { get; }
        IReviewRepository Reviews { get; }
        IFavoriteBookRepository FavoriteBooks { get; }

        Task<int> CompleteAsync();
    }
}