using Deferat.Models;
using Deferat.Services;
using Deferat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Deferat.Components
{
    public class PostDetailsViewComponent : ViewComponent
    {
        private IAuthorService _authorService;

        public PostDetailsViewComponent(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IViewComponentResult Invoke(Post post)
        {
            var author = _authorService.GetAuthor(post.Author);

            var viewModel = new PostAuthorViewModel()
            {
                Post = post,
                Author = author
            };

            return View("Default", viewModel);
            
        }
    }
}