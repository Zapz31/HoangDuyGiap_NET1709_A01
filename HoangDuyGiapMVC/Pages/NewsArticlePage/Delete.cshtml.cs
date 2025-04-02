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
using HoangDuyGiapMVC.Pages.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HoangDuyGiapMVC.Pages.NewsArticlePage
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly IHubContext<SignalRHub> _hubContext;

        public DeleteModel(INewsArticleRepo newsArticleRepo, IHubContext<SignalRHub> hubContext)
        {
            _newsArticleRepo = newsArticleRepo;
            _hubContext = hubContext;
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
            await _hubContext.Clients.All.SendAsync("Change");
            TempData["Message"] = "Delete Succesfull";

            return RedirectToPage("./Index");
        }
    }
}
