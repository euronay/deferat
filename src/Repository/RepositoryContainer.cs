using Deferat.Models;

namespace Deferat.Repository
{
    public class RepositoryContainer : IRepositoryContainer
    {
        public IRepository<Post> Posts { get; set; }
        public IRepository<Author> Authors { get; set; }

        public RepositoryContainer(IRepository<Post> postRepository, IRepository<Author> authorRepository)
        {
            Posts = postRepository;
            Authors = authorRepository;
        }
    }
}