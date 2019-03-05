using System;
using System.Collections.Generic;
using Deferat.Models;

namespace Deferat.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImage { get; set; }
    }
}