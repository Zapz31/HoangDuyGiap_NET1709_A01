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
using System.Drawing;

namespace HoangDuyGiapMVC.Pages.SystemAccountPage
{
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountRepo _context;

        public DetailsModel(ISystemAccountRepo context)
        {
            _context = context;
        }

        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            short idShort = (short)id;

            var systemaccount = await _context.GetAccountByID(idShort);
            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }
    }
}
