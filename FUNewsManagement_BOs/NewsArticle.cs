using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BOs;

public partial class NewsArticle
{
    [Required(ErrorMessage = "NewsArticle Id is required")]
    public string NewsArticleId { get; set; } = null!;
    [Required(ErrorMessage = "NewsArticle Title is required")]
    public string? NewsTitle { get; set; }
    [Required(ErrorMessage = "NewsArticle Headline is required")]
    public string Headline { get; set; } = null!;
    [Required(ErrorMessage = "CreatedDate is required")]
    public DateTime? CreatedDate { get; set; }
    [Required(ErrorMessage = "NewsContent is required")]
    public string? NewsContent { get; set; }
    [Required(ErrorMessage = "NewsSource is required")]
    public string? NewsSource { get; set; }
    [Required(ErrorMessage = "Category is required")]
    public short? CategoryId { get; set; }
    [Required(ErrorMessage = "News Status is required")]
    public bool? NewsStatus { get; set; }
    [Required(ErrorMessage = "Created By is required")]
    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
