using FUNewsManagement_BOs;
using FUNewsManagement_Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FUNewsManagement_DAOs.NewsArticleDAO;

namespace HoangDuyGiapMVC.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly INewsArticleRepo _newsArticleRepo;

        public ProfileModel(ISystemAccountRepo systemAccountRepo, INewsArticleRepo newsArticleRepo)
        {
            _systemAccountRepo = systemAccountRepo;
            _newsArticleRepo = newsArticleRepo;
        }

        public SystemAccount Account { get; set; } = new SystemAccount();
        public List<NewsArticleDto> NewsArticles { get; set; } = new List<NewsArticleDto>();
        public async Task OnGetAsync(short id)
        {
 
                // Retrieve account information
                Account = await _systemAccountRepo.GetAccountByID(id);

                // Retrieve related news articles
                NewsArticles = await _newsArticleRepo.GetNewsArticlesByAccount(id);
         
        }
    }
}
