using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Models
{
    public class Post : HtmlContent
    {
        public ICollection<PostCategory> Categories { get; set; }
        public string FeaturedImage { get; set; }
        public bool IsFeatured { get; set; }
    }
    [Table("PostCategories")]
    public class PostCategory : Metadata
    {
        [Key]
        public string Permalink { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
    public class Metadata
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset Deleted { get; set; }
        public DateTimeOffset Published { get; set; }
        public Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ApplicationUser DeletedBy { get; set; }
        public virtual ApplicationUser PublishedBy { get; set; }
    }
    public class HtmlContent : Metadata
    {
        [Key]
        public string Permalink { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Intro { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeywords { get; set; }
    }
    public enum Status
    {
        Draft,
        Published,
        Review
    }
}