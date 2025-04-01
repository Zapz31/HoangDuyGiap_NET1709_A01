using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FUNewsManagement_BOs;
using FUNewsManagement_DAOs;
using FUNewsManagement_Repos;

namespace HoangDuyGiapMVC.Pages.NewsArticlePage
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;

        public DetailsModel(INewsArticleRepo newsArticleRepo)
        {
            _newsArticleRepo = newsArticleRepo;
        }

        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsArticle = await _newsArticleRepo.GetNewsArticleById(id ?? default(string));
            return Page();
        }
    }
}
