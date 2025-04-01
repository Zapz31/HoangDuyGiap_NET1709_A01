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

namespace HoangDuyGiapMVC.Pages.CategoryPage
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

        private readonly ICategoryRepo _categoryRepo;

        public IndexModel(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _categoryRepo.GetListWithPagination(searchTerm, PageIndex, 10);

            Category = result.Categories;
            PageIndex = result.PageIndex;
            TotalPages = result.TotalPages;

        }
    }
}
