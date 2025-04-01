using FUNewsManagement_BOs;
using FUNewsManagement_Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoangDuyGiapMVC.Pages
{
    public class UserEditProfileModel : PageModel
    {
        private readonly ISystemAccountRepo _systemAccountRepo;

        public UserEditProfileModel(ISystemAccountRepo systemAccountRepo)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {

                await _systemAccountRepo.UpdatePainting(SystemAccount);
                TempData["Message"] = "Update Succesfull";

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Page();
            }
        }
    }
}
