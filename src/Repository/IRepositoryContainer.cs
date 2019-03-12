using Deferat.Models;

namespace Deferat.Repository
{
    public interface IRepositoryContainer
    {
        IRepository<Post> Posts { get; }
        IRepository<Author> Authors { get; }
    }
}