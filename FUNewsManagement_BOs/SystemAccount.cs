using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BOs;

public partial class SystemAccount
{
    public short AccountId { get; set; }
    [Required(ErrorMessage = "Account Name is required")]
    public string? AccountName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email format is example@email.com")]
    public string? AccountEmail { get; set; }
    [Required(ErrorMessage = "Role is required")]
    public int? AccountRole { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
