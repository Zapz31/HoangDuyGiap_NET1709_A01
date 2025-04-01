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

namespace HoangDuyGiapMVC.Pages.CategoryPage
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepo _categoryRepo;

        public CreateModel(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> OnGet()
        {
            var categories = await _categoryRepo.GetList();
        ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                //short nextId = await _categoryRepo.GetNextCategoryIdAsync();
                //Category.CategoryId = nextId;
                await _categoryRepo.AddCategory(Category);
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
