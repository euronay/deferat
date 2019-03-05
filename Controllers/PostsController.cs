
using Deferat.Models;
using Deferat.Repository;
using Deferat.Services;
using Deferat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Deferat.Controllers
{
    public class PostsController : Controller
    {
        private const int PostsPerPage = 3;
        private IRepositoryContainer _repositories;
        public PostsController(IRepositoryContainer repositories)
        {
            _repositories = repositories;
        }

        // GET: Posts
        public ActionResult Index(string tag, int pageNumber = 1)
        {
            var pageCount = 0;

            var posts = _repositories.Posts.Get(orderBy: list => list.OrderByDescending(post => post.Date));
            if(!String.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.Categories.Contains(tag));
            }
            
            pageCount = (posts.Count() - 1) / PostsPerPage + 1;

            if (pageNumber > pageCount)
                throw new ArgumentException($"Requested page {pageNumber} but there are only {pageCount} pages");

            posts = posts.Skip((pageNumber - 1) * PostsPerPage).Take(PostsPerPage);


            var viewModel = new PostListViewModel()
            {
                Posts = posts.Select(p => CreatePostViewModel(p)),
                PageCount = pageCount,
                CurrentPage = pageNumber
            };

            return View(viewModel);
        }

        public ActionResult Read(string id)
        {
            var post = _repositories.Posts.Get(id.ToLower());
            if(post == null)
                return new StatusCodeResult(404);
            
            return View(CreatePostViewModel(post));

        }

        private PostViewModel CreatePostViewModel(Post post)
        {
            PostViewModel postViewModel = new PostViewModel()
            {
                Post = post
            };

            var author = _repositories.Authors.Get(post.Author.ToLower());
            
            postViewModel.AuthorName = author != null ? author.DisplayName : "Anonymous";
            postViewModel.AuthorImage = author != null ? author.Image : "";

            return postViewModel;         
        }

    }
}