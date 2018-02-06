using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Training.ViewModels
{
    public class Post
    {
        public string Permalink { get; set; }

        [Required(ErrorMessage = "Judul tidak boleh kosong")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Intro tidak boleh kosong")]
        [AllowHtml]
        public string Intro { get; set; }

        [Required(ErrorMessage = "Konten tidak boleh kosong")]
        [AllowHtml]
        public string Content { get; set; }

        public string Category { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeywords { get; set; }
        public HttpPostedFileBase FeaturedImage { get; set; }
        public ICollection<string> Categories { get; set; }
    }
    public class PostCategory
    {
        public string Permalink { get; set; }
        [Required(ErrorMessage = "Nama kategori tidak boleh kosong")]
        [Display(Name = "Nama Kategori")]
        public string Title { get; set; }
        [Display(Name = "Urutan")]
        public int Order { get; set; }
        
        public PostCategory()
        {
            // Default Constructor
        }

        public PostCategory(Models.PostCategory _category)
        {
            Permalink = _category.Permalink;
            Title = _category.Title;
            Order = _category.Order;
        }
    }
}