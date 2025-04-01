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

namespace HoangDuyGiapMVC.Pages.SystemAccountPage
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountRepo _systemAccountRepo;
        public List<SelectListItem> RoleList { get; set; }

        public EditModel(ISystemAccountRepo systemAccountRepo)
        {
            _systemAccountRepo = systemAccountRepo;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int idInt = (short)id;
            var systemaccount = await _systemAccountRepo.GetAccountByID(idInt);
            if (systemaccount == null)
            {
                return NotFound();
            }
            SystemAccount = systemaccount;
            RoleList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Staff" },
                    new SelectListItem { Value = "2", Text = "Lecturer" }
                };
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                await _systemAccountRepo.UpdatePainting(SystemAccount);
                TempData["Message"] = "Update Succesfull";

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Page();
            }
        }
    }
}
