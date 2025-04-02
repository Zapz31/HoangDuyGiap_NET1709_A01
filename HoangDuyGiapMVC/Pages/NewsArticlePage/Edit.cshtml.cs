using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FUNewsManagement_BOs;
using FUNewsManagement_DAOs;
using FUNewsManagement_Repos;
using HoangDuyGiapMVC.Pages.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HoangDuyGiapMVC.Pages.NewsArticlePage
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly IHubContext<SignalRHub> _hubContext;

        public EditModel(INewsArticleRepo newsArticleRepo, ICategoryRepo categoryRepo, ISystemAccountRepo systemAccountRepo, IHubContext<SignalRHub> hubContext)
        {
            _categoryRepo = categoryRepo;
            _newsArticleRepo = newsArticleRepo;
            _systemAccountRepo = systemAccountRepo;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Step 1: Check if session "id" is null or empty
            var sessionId = HttpContext.Session.GetInt32("id");
            if (sessionId == null || sessionId.ToString().Trim() == "")
            {
                return RedirectToPage("/Login");
            }
            if (id == null)
            {
                return NotFound();
            }


            // Step 2: If session "id" is 7, skip further checks
            if (sessionId != 7)
            {
                // Step 3.1: Retrieve the account object using sessionId
                var accA = await _systemAccountRepo.GetAccountByID(sessionId.Value);
                if (accA == null)
                {
                    return RedirectToPage("/Error");
                }

                // Step 3.2: Fetch the news article and check CreatedById
                var newsArticle = await _newsArticleRepo.GetNewsArticleById(id);
                if (newsArticle == null)
                {
                    return NotFound();
                }

                if (accA.AccountId != newsArticle.CreatedById)
                {
                    return RedirectToPage("/Error");
                }

                NewsArticle = newsArticle;
            }
            else
            {
                // Admin case: Fetch the article normally
                var newsArticle = await _newsArticleRepo.GetNewsArticleById(id);
                if (newsArticle == null)
                {
                    return NotFound();
                }
                NewsArticle = newsArticle;
            }
            var categoryList = await _categoryRepo.GetList();
            var createdByList = await _systemAccountRepo.GetList();
           ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
           ViewData["CreatedById"] = new SelectList(createdByList, "AccountId", "AccountId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var sessionId = HttpContext.Session.GetInt32("id");
            if (sessionId == null || sessionId.ToString().Trim() == "")
            {
                return RedirectToPage("/Login");
            }

            try
            {
                NewsArticle.ModifiedDate = DateTime.Now;
                // Convert NewsStatus from string to boolean (if needed)
                if (Request.Form.TryGetValue("NewsArticle.NewsStatus", out var newsStatusValue))
                {
                    NewsArticle.NewsStatus = bool.TryParse(newsStatusValue, out var status) ? status : (bool?)null;
                }
                NewsArticle.UpdatedById = (short?)sessionId;
                await _newsArticleRepo.UpdateNewsAricle(NewsArticle);
                await _hubContext.Clients.All.SendAsync("Change");
                TempData["Message"] = "Update Succesfull";

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private bool NewsArticleExists(string id)
        {
            return _newsArticleRepo.GetNewsArticleById(id) != null;
        }
    }
}
