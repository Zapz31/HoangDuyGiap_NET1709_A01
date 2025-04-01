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

namespace HoangDuyGiapMVC.Pages.SystemAccountPage
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountRepo _systemAccountRepo;

        public List<SelectListItem> RoleList { get; set; }

        public CreateModel(ISystemAccountRepo systemAccountRepo)
        {
            _systemAccountRepo = systemAccountRepo;
        }

        public async Task<IActionResult> OnGet()
        {
            RoleList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecturer" }
            };
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           

            try
            {
                SystemAccount.AccountId = await _systemAccountRepo.GetNextAccountId();
                await _systemAccountRepo.CreateAccount(SystemAccount);
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
