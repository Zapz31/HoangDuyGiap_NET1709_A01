using FUNewsManagement_Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoangDuyGiapMVC.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string email { get; set; }

        [BindProperty]
        public string password { get; set; }
        
        private readonly ISystemAccountRepo _accountRepo;
        private readonly IConfiguration _configuration;
        public LoginModel(ISystemAccountRepo accountRepo, IConfiguration configuration)
        {
            _accountRepo = accountRepo;
            _configuration = configuration;
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                var adminID = _configuration["AdminAccount:AccountID"];
                var adminEmail = _configuration["AdminAccount:email"];
                var adminPassword = _configuration["AdminAccount:password"];
                var adminAccountName = _configuration["AdminAccount:AccountName"];

                // Check if the entered credentials match the admin account
                if (email == adminEmail && password == adminPassword)
                {
                    TempData["Message"] = "Admin Login Success";
                    Console.WriteLine("Admin Login Success");

                    // Set session for admin
                    if (adminAccountName != null && adminID != null)
                    {
                        HttpContext.Session.SetString("AccountName", adminAccountName);
                        HttpContext.Session.SetInt32("id", Convert.ToInt32(adminID));
                    }
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetInt32("RoleId", 3); 

                    return RedirectToPage("/NewsReportStatistics");
                }else
                {
                    var account = await _accountRepo.Login(email, password);
                    if (account != null)
                    {
                        TempData["Message"] = "Login Success";
                        Console.WriteLine("Login Success");

                        //set session
                        HttpContext.Session.SetInt32("id", account.AccountId);
                        HttpContext.Session.SetString("Email", email);
                        HttpContext.Session.SetInt32("RoleId", account.AccountRole ?? default(int));
                        if (account.AccountName != null)
                        {
                            HttpContext.Session.SetString("AccountName", account.AccountName);
                        }

                        return RedirectToPage("/NewsArticlePage/Index");
                    }
                    else
                    {
                        TempData["Message"] = "You do not have permission to do this function";
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Page();
            }
        }
    }


}
