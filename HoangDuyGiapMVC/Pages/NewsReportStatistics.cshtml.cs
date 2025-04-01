using FUNewsManagement_DAOs;
using FUNewsManagement_Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FUNewsManagement_DAOs.NewsArticleDAO;

namespace HoangDuyGiapMVC.Pages
{
    public class NewsReportStatisticsModel : PageModel
    {
        private readonly INewsArticleRepo _articlesRepo;

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<NewsReportStatistic> ReportStatistics { get; set; }

        public NewsReportStatisticsModel(INewsArticleRepo newsArticleRepo)
        {
            _articlesRepo = newsArticleRepo;
            // Set default date range to last 30 days
            EndDate = DateTime.Now;
            StartDate = EndDate.AddDays(-30);
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Load report when date range is submitted
            await LoadReportStatistics();
            return Page();
        }

        private async Task LoadReportStatistics()
        {
            try
            {
                ReportStatistics = await _articlesRepo.GetReportStatisticsAsync(StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error loading report: " + ex.Message);
                ReportStatistics = new List<NewsReportStatistic>();
            }
        }


    }
}
