using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Training.ViewModels
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Intro { get; set; }
    }
    public class PostCategory
    {
        [Required(ErrorMessage = "Nama kategori tidak boleh kosong")]
        [Display(Name = "Nama Kategori")]
        public string Title { get; set; }
        [Display(Name = "Urutan")]
        public int Order { get; set; }
    }
}