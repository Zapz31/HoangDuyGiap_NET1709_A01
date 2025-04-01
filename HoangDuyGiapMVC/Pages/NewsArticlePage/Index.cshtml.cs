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
    public class IndexModel : PageModel
    {
        //search
        [BindProperty(SupportsGet = true)]
        public string searchTerm { get; set; }

        //paging
        public int PageIndex { get; set; } = 1;

        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 2;

        private readonly INewsArticleRepo _newsArticleRepo;

        public IndexModel(INewsArticleRepo newsArticleRepo)
        {
            _newsArticleRepo = newsArticleRepo;
        }

        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        public async Task OnGetAsync(int pageIndex = 1)
        {
            int? roleID = HttpContext.Session.GetInt32("RoleId");
            int notNullID = (int)roleID; 
            var result = await _newsArticleRepo.GetList(searchTerm, pageIndex, 5, notNullID);

            NewsArticle = result.NewsArticles;
            PageIndex = result.PageIndex;
            TotalPages = result.TotalPages;
        }

    }
}
