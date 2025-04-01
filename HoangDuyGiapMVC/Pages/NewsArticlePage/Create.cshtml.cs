using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagement_BOs;
using FUNewsManagement_DAOs;
using FUNewsManagement_Repos;
using Microsoft.AspNetCore.SignalR;
using HoangDuyGiapMVC.Pages.Hubs;

namespace HoangDuyGiapMVC.Pages.NewsArticlePage
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ITagRepo _tagRepo;
        private readonly IHubContext<SignalRHub> _hubContext;
        public CreateModel(ISystemAccountRepo systemAccountRepo, ICategoryRepo categoryRepo, INewsArticleRepo newsArticleRepo, ITagRepo tagRepo, IHubContext<SignalRHub> hubContext)
        {
            _categoryRepo = categoryRepo;
            _systemAccountRepo = systemAccountRepo;
            _newsArticleRepo = newsArticleRepo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGet()
        {
            var categoryList = await _categoryRepo.GetList();
            var systemAccount = await _systemAccountRepo.GetList();
            var tagList = await _tagRepo.GetList();
            ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewData["CreatedById"] = new SelectList(systemAccount, "AccountId", "AccountName");
            ViewData["TagList"] = new SelectList(tagList, "TagId", "TagName");
            return Page();
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>(); // Danh sách Tag được chọn

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var existingTags = await _tagRepo.GetTagsByIds(SelectedTagIds);
                if (existingTags == null || !existingTags.Any())
                {
                    TempData["Message"] = "Please choose at least one tag for your news";
                    return Page();
                }
                    var sessionId = HttpContext.Session.GetInt32("id");
                if (sessionId == null || sessionId.ToString().Trim() == "")
                {
                    return RedirectToPage("/Login");
                }
                short intSessionId = (short)sessionId;
                NewsArticle.NewsArticleId = await _newsArticleRepo.GetNextNewsArticleID();
                NewsArticle.CreatedById = intSessionId;
                NewsArticle.CreatedDate = DateTime.Now;
                // Lấy danh sách Tags được chọn
                
                await _newsArticleRepo.AddNewsArticle(NewsArticle, existingTags);
                await _hubContext.Clients.All.SendAsync("Change");
                TempData["Message"] = "Create Succesfull";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return await OnGet();
            }
        }
    }
}
