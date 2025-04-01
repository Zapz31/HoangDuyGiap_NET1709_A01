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
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;

        public DeleteModel(INewsArticleRepo newsArticleRepo)
        {
            _newsArticleRepo = newsArticleRepo;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = await _newsArticleRepo.GetNewsArticleById(id);

            if (newsarticle == null)
            {
                return NotFound();
            }
            else
            {
                NewsArticle = newsarticle;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _newsArticleRepo.DeleteNewsArticle(id);
            TempData["Message"] = "Delete Succesfull";

            return RedirectToPage("./Index");
        }
    }
}
