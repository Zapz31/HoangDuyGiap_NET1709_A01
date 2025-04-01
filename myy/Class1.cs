/*
 
Muc luc
Here are the headings extracted from the image:
1.	REFERENCES (this session just for reference, student...)

Nuget install
DB generation
Appsetting.json

2.	DB context: 
o	
1.	Login:
	1.1. Login.cshtml
	1.2. Login.cshtml.cs
o	
2.	DAO====
DAO init
DAO get all list
DAO pagination object response
DAO get List Pagination + sort
DAO get object by id
DAO update object
DAO delete object
DAO add object

o	
3.	REPO
	3.1. repo implement
	repo implement 
	repo implement add object
	repo implement delete object
	repo implement get all
	repo implement get all with pagination
	repo implement get object by id
	repo implement update object
	3.2. repo interface
o	
4.	Program.cs
o	
5.	ERROR PAGE (Error.cshtml)
o	
6.	UseSession on each razorPage
o	
7.	RAZOR PAGE
	7.1. Create 
	7.1.1. Create.cshtml
	7.1.2. Create.cshtml.cs
	7.2. Index.cshtml (View All List) 
	7.2.1. Index.cshtml
	7.2.2. Index.cshtml.cs
	7.3. Details 
	7.3.1. Details.cshtml
	7.3.2. Details.cshtml.cs
	7.4. Edit 
	7.4.1. Edit.cshtml
	7.4.2. Edit.cshtml.cs
	7.5. Delete 
	7.5.1. Delete.cshtml
	7.5.2. Delete.cshtml.cs
3.	Validation-bos
full-bos
some regex
Let me know if you need any modifications or additional details! 🚀

PE_PRN221_FA24_TrailTest_Note 

Please read the instructions carefully before doing the questions.
•	You are NOT allowed to use any other materials. You are NOT allowed to use any device to share data with others.
•	You must use Visual Studio 2019 or above, MSSQL Server 2008 or above for your development tools.  
IMPORTANT – before you start doing your solution, MUST do the following steps:
1.	Create Solution in Visual Studio named PE_PRN221_FA24_TrailTest_StudentName.  Inside your Solution, the Project ASP.NET Core Razor Page must be named: Euro2024DB_StudentName
2.	Create your MS SQL database named Euro2024DB by running code in script Euro2024DB.sql
3.	Set the default user interface for your project as Login page.
4.	To do your program, you must use ASP.NET Core Razor Pages. Note that you are not allow to connect direct to database from Razor Pages, every database connection must be used through Repository and Data Access Objects. The database connection string must get from appsettings.json file.
In the case your code connects directly to database from ASP.NET Core Razor Pages or you hardcode connection string, you will get 0 mark.
5.	If there are syntax errors or compilation errors in your PE program, you will not pass the PE requirements, the mark will be 0.
6.	Your work will be considered invalid (0 point) if your code inserts stuff that is unrelated to the test.


REFERENCES (this session just for reference, student can use other approach to do the practical exam)
Note
- Install package using CLI or Power Shell 
	Microsoft.EntityFrameworkCore.SqlServer version	Microsoft.Extensions.Configuration,  Microsoft.Extensions.Configuration.Json version
.NET 5	5.0.17	5.0.0
.NET 6	6.0.27	6.0.1/6.0.0
.NET 7	7.0.16	7.0.0
.NET 8	8.0.2	8.0.0

Nuget install
====================================================================
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.2



dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.0
=======================================================================

- Connection String
"Server=(local);Uid=sa;Pwd=1234567890;Database=Euro2024DB; TrustServerCertificate=True"



Entity Framework Core
- Install dotnet-ef for CLI
dotnet tool install --global dotnet-ef --version 5.0.17

- Use Entity Framework Core to generate Object Model from existing database – CLI
dotnet ef dbcontext scaffold "Server=(local);Uid=sa;Pwd=1234567890;Database=Euro2024DB; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir ./

- Generate database from domain classes – CLI.
dotnet ef migrations  add "InitialDB" 
dotnet ef database update



Entity Framework Core
- Use Entity Framework Core to generate Object Model from existing database – Package Manager Console
Scaffold-DbContext "Server=(local);Database=Euro2024DB;Uid=sa;Pwd=1234567890;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir ./

- Generate database from domain classes – Package Manager Console
Add-Migration  "InitialDB" 
Update-Database -verbose


Json: 
Appsetting.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DBConnection": "Server=(local);Uid=sa;Pwd=12345;Database=Sp25PharmaceuticalDB; TrustServerCertificate=True"
  }
}


DB context: 
    private string? GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config.GetConnectionString("DBConnection");

        return strConn;
    }
DB generation
dotnet ef dbcontext scaffold "Server=(local);Uid=sa;Pwd=12345;Database=Sp25PharmaceuticalDB; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir ./

1. Login: 
1.1.Login.cshtml:
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
}

<div class="text-center">
    <h1 class="display-4">Login</h1>

    @if(TempData["Message"] != null)
    {
        <div class="text-danger">@TempData["Message"]</div>
    }

    <form method="post">
        <div>
            <label>Email:</label>
            <input name="email" type="text" />
        </div>
        <div class="my-2">
            <label>Password:</label>
            <input name="password" type="password" />
        </div>
        <input type="submit" value="Login" />
    </form>
    
</div>

1.2.Login.cshtml.cs:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace OilPaintingArt_UyDev.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string email { get; set; }

        [BindProperty]
        public string password { get; set; }

        private readonly IAccountRepo _accountRepo;

        public IndexModel(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var account = await _accountRepo.Login(email, password);
                if (account != null && (account.Role == 2 || account.Role == 3))
                {
                    TempData["Message"] = "Login Success";
                    Console.WriteLine("Login Success");

                    //set session
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetInt32("RoleId", account.Role ?? default(int));

                    return RedirectToPage("/OilPaintingArtPage/Index");
                }
                else
                {
                    TempData["Message"] = "You do not have permission to do this function";
                    return Page();
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


===============================================================================

2.DAO=====
using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
    public class OilPaintingArtDAO
    {
 DAO init 
=======================================
        private readonly OilPaintingArt2024DBContext _context;
        private static OilPaintingArtDAO instance = null;

        private OilPaintingArtDAO()
        {
            _context = new OilPaintingArt2024DBContext();
        }

        public static OilPaintingArtDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    return new OilPaintingArtDAO();
                }
                return instance;
            }
        }
========================================
DAO get all list
===============================================================
        public async Task<List<OilPaintingArt>> GetList()
        {
            return await _context.OilPaintingArts.Include(x => x.Supplier).ToListAsync();
        }
===============================================================

DAO pagination object response
===============================================================
        public class PaintingResponse
        {
            public List<OilPaintingArt> OilPaintingArts { get; set; }
            public int TotalPages { get; set; }
            public int PageIndex { get; set; }
        }

DAO get List Pagination + sort
=========================================================================
        public async Task<PaintingResponse> GetList(string searchTerm, int pageIndex, int pageSize)
        {
            var query = _context.OilPaintingArts.Include(x => x.Supplier).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.OilPaintingArtStyle.ToLower().Contains(searchTerm.ToLower()) || x.Artist.ToLower().Contains(searchTerm.ToLower()));
            }

	// Sort if need
	- Tang dan
	query = query.OrderByDescending(x => x.Id);
	- giam dan
query = query.OrderBy(x => x.Id);
- nhieu tieu chi
query = query.OrderBy(x => x.Id) // tang dan theo id
.ThenByDescending(x => x.CreatedDate) // giam dan theo CreatedDate 
.ThenBy(x => x.NewsTitle); tang dan theo NewsTitle

            int count = await query.CountAsync(); //11
            int totalPages = (int)Math.Ceiling(count / (double)pageSize); //3

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PaintingResponse
            {
                OilPaintingArts = await query.ToListAsync(),
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }
DAO get object by id

        public async Task<OilPaintingArt> GetOilPaintingArtById(int id)
        {
            return await _context.OilPaintingArts.Include(x => x.Supplier).FirstOrDefaultAsync(m => m.OilPaintingArtId == id);
        }
DAO add object
        public async Task AddPainting(OilPaintingArt oilPaintingArt)
        {
            try
            {
                var existingArt = await GetOilPaintingArtById(oilPaintingArt.OilPaintingArtId);
                if (existingArt != null)
                {
                    throw new Exception("Art already exists");
                }
                _context.OilPaintingArts.Add(oilPaintingArt);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

DAO update object
        public async Task UpdatePainting(OilPaintingArt oilPaintingArt)
        {
            try
            {
                var existingArt = await GetOilPaintingArtById(oilPaintingArt.OilPaintingArtId);
                if (existingArt == null)
                {
                    throw new Exception("Does not exist");
                }

                existingArt.ArtTitle = oilPaintingArt.ArtTitle;
                existingArt.OilPaintingArtLocation = oilPaintingArt.OilPaintingArtLocation;
                existingArt.OilPaintingArtStyle = oilPaintingArt.OilPaintingArtStyle;
                existingArt.Artist = oilPaintingArt.Artist;
                existingArt.NotablFeatures = oilPaintingArt.NotablFeatures;
                existingArt.PriceOfOilPaintingArt = oilPaintingArt.PriceOfOilPaintingArt;
                existingArt.StoreQuantity = oilPaintingArt.StoreQuantity;
                existingArt.CreatedDate = oilPaintingArt.CreatedDate; //DateTime.Now;

                var supplier = await _context.SupplierCompanies.FirstOrDefaultAsync(s => s.SupplierId == oilPaintingArt.SupplierId);
                if (supplier == null)
                {
                    throw new Exception("Supplier does not exist");
                }
                existingArt.SupplierId = oilPaintingArt.SupplierId;

                _context.OilPaintingArts.Update(existingArt);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
==========================================================================
DAO delete object

        public async Task DeleteArt(int id)
        {
            try
            {
                var existArt = _context.OilPaintingArts.FirstOrDefault(m => m.OilPaintingArtId == id);
                if (existArt == null)
                {
                    throw new Exception("Art not found");
                }
                _context.OilPaintingArts.Remove(existArt);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<SupplierCompany>> GetSuppilers()
        {
            return await _context.SupplierCompanies.ToListAsync();
        }
    }
}
=====================================================================

3.REPO
3.1.Class implement: 
repo implement
using BOs;
using DAOs;

namespace Repos
{
    public class ArtRepo : IArtRepo
    {
repo implement add object
        public Task AddPainting(OilPaintingArt oilPaintingArt)
        {
            return OilPaintingArtDAO.Instance.AddPainting(oilPaintingArt);
        }
repo implement delete object
        public Task DeletePainting(int id)
        {
            return OilPaintingArtDAO.Instance.DeleteArt(id);
        }
repo implement get all
        public Task<List<OilPaintingArt>> GetList()
        {
            return OilPaintingArtDAO.Instance.GetList();
        }
repo implement get all with pagination
        public Task<OilPaintingArtDAO.PaintingResponse> GetList(string searchTerm, int pageIndex, int pageSize)
        {
            return OilPaintingArtDAO.Instance.GetList(searchTerm, pageIndex, pageSize);
        }
========================================================
repo implement get object by id
        public Task<OilPaintingArt> GetOilPaintingArtById(int id)
        {
            return OilPaintingArtDAO.Instance.GetOilPaintingArtById(id);
        }
==========================================================
repo implement update object
        public Task UpdatePainting(OilPaintingArt oilPaintingArt)
        {
            return OilPaintingArtDAO.Instance.UpdatePainting(oilPaintingArt);
        }
===========================================================
    }
}

3.2.Interface
repo interface
using BOs;
using static DAOs.OilPaintingArtDAO;

namespace Repos
{
    public interface IArtRepo
    {
        Task<List<OilPaintingArt>> GetList();

        Task<PaintingResponse> GetList(string searchTerm, int pageIndex, int pageSize);
        Task<OilPaintingArt> GetOilPaintingArtById(int id);
        Task AddPainting(OilPaintingArt oilPaintingArt);
        Task UpdatePainting(OilPaintingArt oilPaintingArt);
        Task DeletePainting(int id);
    }
}


===============================================================================
4.Program.cs:
using Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//DI - Dependency Injection
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IArtRepo, ArtRepo>();
builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();

//Add Session
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

//Use Session
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
5.ERROR PAGE (Error.cshtml):
@page
@model ErrorModel
@{
    ViewData["Title"] = "Error";
}


@if(TempData["ErrorMessage"] != null)
{
    <h2 class="text-danger">@TempData["ErrorMessage"]</h2>
}

6.UseSession on each razorPage:
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.CreateModel

@{
    ViewData["Title"] = "Create";

    string? email = HttpContext.Session.GetString("Email");
    int? role = HttpContext.Session.GetInt32("RoleId");

    if(email == null || (role != 3))
    {
        TempData["ErrorMessage"] = "You do not have permission to do this function";
        Response.Redirect("/Error");
    }
}

<h1>Create</h1>


@if (TempData["Message"] != null)
{
    <h2 class="text-info">@TempData["Message"]</h2>
}
7.RAZOR PAGE
7.1.Create
7.1.1.Create.cshtml
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.CreateModel

@{
    ViewData["Title"] = "Create";

    string? email = HttpContext.Session.GetString("Email");
    int? role = HttpContext.Session.GetInt32("RoleId");

    if(email == null || (role != 3))
    {
        TempData["ErrorMessage"] = "You do not have permission to do this function";
        Response.Redirect("/Error");
    }
}

<h1>Create</h1>


@if (TempData["Message"] != null)
{
    <h2 class="text-info">@TempData["Message"]</h2>
}

<h4>OilPaintingArt</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.OilPaintingArtId" class="control-label"></label>
                <input asp-for="OilPaintingArt.OilPaintingArtId" class="form-control" />
                <span asp-validation-for="OilPaintingArt.OilPaintingArtId" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.ArtTitle" class="control-label"></label>
                <input asp-for="OilPaintingArt.ArtTitle" class="form-control" />
                <span asp-validation-for="OilPaintingArt.ArtTitle" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.OilPaintingArtLocation" class="control-label"></label>
                <input asp-for="OilPaintingArt.OilPaintingArtLocation" class="form-control" />
                <span asp-validation-for="OilPaintingArt.OilPaintingArtLocation" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.OilPaintingArtStyle" class="control-label"></label>
                <input asp-for="OilPaintingArt.OilPaintingArtStyle" class="form-control" />
                <span asp-validation-for="OilPaintingArt.OilPaintingArtStyle" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.Artist" class="control-label"></label>
                <input asp-for="OilPaintingArt.Artist" class="form-control" />
                <span asp-validation-for="OilPaintingArt.Artist" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.NotablFeatures" class="control-label"></label>
                <input asp-for="OilPaintingArt.NotablFeatures" class="form-control" />
                <span asp-validation-for="OilPaintingArt.NotablFeatures" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.PriceOfOilPaintingArt" class="control-label"></label>
                <input asp-for="OilPaintingArt.PriceOfOilPaintingArt" class="form-control" />
                <span asp-validation-for="OilPaintingArt.PriceOfOilPaintingArt" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.StoreQuantity" class="control-label"></label>
                <input asp-for="OilPaintingArt.StoreQuantity" class="form-control" />
                <span asp-validation-for="OilPaintingArt.StoreQuantity" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.CreatedDate" class="control-label"></label>
                <input asp-for="OilPaintingArt.CreatedDate" class="form-control" />
                <span asp-validation-for="OilPaintingArt.CreatedDate" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.SupplierId" class="control-label"></label>
                <select asp-for="OilPaintingArt.SupplierId" class ="form-control" asp-items="ViewBag.SupplierId"></select>
            </div>
            <div class="form-group">
                <input type="submit" value="Create" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-page="Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
7.1.2.Create.cshtml.cs
using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repos;

namespace OilPaintingArt_UyDev.Pages.OilPaintingArtPage
{
    public class CreateModel : PageModel
    {
        private readonly IArtRepo _artRepo;
        private readonly ISupplierRepo _supplierRepo;

        public CreateModel(IArtRepo artRepo, ISupplierRepo supplierRepo)
        {
            _artRepo = artRepo;
            _supplierRepo = supplierRepo;
        }

        public async Task<IActionResult> OnGet()
        {
            var listItem = await _supplierRepo.GetList();
            ViewData["SupplierId"] = new SelectList(listItem, "SupplierId", "CompanyName");
            return Page();
        }

        [BindProperty]
        public OilPaintingArt OilPaintingArt { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _artRepo.AddPainting(OilPaintingArt);
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
7.2.Index.cshtml (View All List)
7.2.1.Index.cshtml
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.IndexModel

@{
    ViewData["Title"] = "Index";
}

<h1>Index</h1>

<p>
    <a asp-page="Create">Create New</a>
</p>

@if(TempData["Message"] != null)
{
    <h2 class="text-info">@TempData["Message"]</h2>
}


<form method="get">
    <input type="text" name="searchTerm" />
    <input type="submit" value="Search" />
</form>


<table class="table">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].ArtTitle)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].OilPaintingArtLocation)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].OilPaintingArtStyle)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].Artist)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].NotablFeatures)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].PriceOfOilPaintingArt)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].StoreQuantity)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].CreatedDate)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.OilPaintingArt[0].Supplier.CompanyName)
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model.OilPaintingArt) {
        <tr>
            <td>
                @Html.DisplayFor(modelItem => item.ArtTitle)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.OilPaintingArtLocation)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.OilPaintingArtStyle)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Artist)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.NotablFeatures)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.PriceOfOilPaintingArt)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.StoreQuantity)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.CreatedDate)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Supplier.CompanyName)
            </td>
            <td>
                <a asp-page="./Edit" asp-route-id="@item.OilPaintingArtId">Edit</a> |
                <a asp-page="./Details" asp-route-id="@item.OilPaintingArtId">Details</a> |
                <a asp-page="./Delete" asp-route-id="@item.OilPaintingArtId">Delete</a>
            </td>
        </tr>
}
    </tbody>
</table>

<ul class="pagination">
    @for(var i = 1; i<=Model.TotalPages; i++)
    {
        <li class="page-item @(i == Model.PageIndex ? "active" : "")">
            <a class="page-link" asp-route-pageIndex="@i">@i</a>
        </li>
    }
</ul>

7.2.2.Index.cshtml.cs
using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace OilPaintingArt_UyDev.Pages.OilPaintingArtPage
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

        private readonly IArtRepo _artRepo;

        public IndexModel(IArtRepo artRepo)
        {
            _artRepo = artRepo;
        }

        public IList<OilPaintingArt> OilPaintingArt { get; set; } = default!;

        public async Task OnGetAsync(int pageIndex = 1)
        {
            var result = await _artRepo.GetList(searchTerm, pageIndex, 2);

            OilPaintingArt = result.OilPaintingArts;
            PageIndex = result.PageIndex;
            TotalPages = result.TotalPages;
        }
    }
}

7.3.Details
7.3.1.Details.cshtml
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.DetailsModel

@{
    ViewData["Title"] = "Details";
}

<h1>Details</h1>

<div>
    <h4>OilPaintingArt</h4>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.ArtTitle)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.ArtTitle)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.OilPaintingArtLocation)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.OilPaintingArtLocation)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.OilPaintingArtStyle)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.OilPaintingArtStyle)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.Artist)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.Artist)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.NotablFeatures)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.NotablFeatures)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.PriceOfOilPaintingArt)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.PriceOfOilPaintingArt)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.StoreQuantity)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.StoreQuantity)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.CreatedDate)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.CreatedDate)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.Supplier.CompanyName)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.Supplier.CompanyName)
        </dd>
    </dl>
</div>
<div>
    <a asp-page="./Edit" asp-route-id="@Model.OilPaintingArt?.OilPaintingArtId">Edit</a> |
    <a asp-page="./Index">Back to List</a>
</div>

7.3.2.Details.cshtml.cs
using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace OilPaintingArt_UyDev.Pages.OilPaintingArtPage
{
    public class DetailsModel : PageModel
    {
        private readonly IArtRepo _artRepo;

        public DetailsModel(IArtRepo artRepo)
        {
            _artRepo = artRepo;
        }

        public OilPaintingArt OilPaintingArt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            OilPaintingArt = await _artRepo.GetOilPaintingArtById(id ?? default(int));
            return Page();
        }
    }
}
7.4.Edit
7.4.1.Edit.cshtml
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.EditModel

@{
    ViewData["Title"] = "Edit";

    string? email = HttpContext.Session.GetString("Email");
    int? role = HttpContext.Session.GetInt32("RoleId");

    if (email == null || (role != 3))
    {
        TempData["ErrorMessage"] = "You do not have permission to do this function";
        Response.Redirect("/Error");
    }
}

<h1>Edit</h1>

<h4>OilPaintingArt</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="OilPaintingArt.OilPaintingArtId" />
            <div class="form-group">
                <label asp-for="OilPaintingArt.ArtTitle" class="control-label"></label>
                <input asp-for="OilPaintingArt.ArtTitle" class="form-control" />
                <span asp-validation-for="OilPaintingArt.ArtTitle" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.OilPaintingArtLocation" class="control-label"></label>
                <input asp-for="OilPaintingArt.OilPaintingArtLocation" class="form-control" />
                <span asp-validation-for="OilPaintingArt.OilPaintingArtLocation" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.OilPaintingArtStyle" class="control-label"></label>
                <input asp-for="OilPaintingArt.OilPaintingArtStyle" class="form-control" />
                <span asp-validation-for="OilPaintingArt.OilPaintingArtStyle" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.Artist" class="control-label"></label>
                <input asp-for="OilPaintingArt.Artist" class="form-control" />
                <span asp-validation-for="OilPaintingArt.Artist" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.NotablFeatures" class="control-label"></label>
                <input asp-for="OilPaintingArt.NotablFeatures" class="form-control" />
                <span asp-validation-for="OilPaintingArt.NotablFeatures" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.PriceOfOilPaintingArt" class="control-label"></label>
                <input asp-for="OilPaintingArt.PriceOfOilPaintingArt" class="form-control" />
                <span asp-validation-for="OilPaintingArt.PriceOfOilPaintingArt" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.StoreQuantity" class="control-label"></label>
                <input asp-for="OilPaintingArt.StoreQuantity" class="form-control" />
                <span asp-validation-for="OilPaintingArt.StoreQuantity" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.CreatedDate" class="control-label"></label>
                <input asp-for="OilPaintingArt.CreatedDate" class="form-control" />
                <span asp-validation-for="OilPaintingArt.CreatedDate" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="OilPaintingArt.SupplierId" class="control-label"></label>
                <select asp-for="OilPaintingArt.SupplierId" class="form-control" asp-items="ViewBag.SupplierId"></select>
                <span asp-validation-for="OilPaintingArt.SupplierId" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Save" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-page="./Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
7.4.2.Edit.cshtml.cs
using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repos;

namespace OilPaintingArt_UyDev.Pages.OilPaintingArtPage
{
    public class EditModel : PageModel
    {
        private readonly IArtRepo _artRepo;
        private readonly ISupplierRepo _supplierRepo;

        public EditModel(IArtRepo artRepo, ISupplierRepo supplierRepo)
        {
            _artRepo = artRepo;
            _supplierRepo = supplierRepo;
        }

        [BindProperty]
        public OilPaintingArt OilPaintingArt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oilpaintingart = await _artRepo.GetOilPaintingArtById(id ?? default(int));
            if (oilpaintingart == null)
            {
                return NotFound();
            }
            OilPaintingArt = oilpaintingart;

            var list = await _supplierRepo.GetList();

            ViewData["SupplierId"] = new SelectList(list, "SupplierId", "CompanyName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                await _artRepo.UpdatePainting(OilPaintingArt);
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
7.5.Delete
7.5.1.Delete.cshtml
@page
@model OilPaintingArt_UyDev.Pages.OilPaintingArtPage.DeleteModel

@{
    ViewData["Title"] = "Delete";

    string? email = HttpContext.Session.GetString("Email");
    int? role = HttpContext.Session.GetInt32("RoleId");

    if (email == null || (role != 3))
    {
        TempData["ErrorMessage"] = "You do not have permission to do this function";
        Response.Redirect("/Error");
    }
}

<h1>Delete</h1>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>OilPaintingArt</h4>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.ArtTitle)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.ArtTitle)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.OilPaintingArtLocation)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.OilPaintingArtLocation)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.OilPaintingArtStyle)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.OilPaintingArtStyle)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.Artist)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.Artist)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.NotablFeatures)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.NotablFeatures)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.PriceOfOilPaintingArt)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.PriceOfOilPaintingArt)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.StoreQuantity)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.StoreQuantity)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.CreatedDate)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.CreatedDate)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.OilPaintingArt.Supplier.CompanyName)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.OilPaintingArt.Supplier.CompanyName)
        </dd>
    </dl>
    
    <form method="post">
        <input type="hidden" asp-for="OilPaintingArt.OilPaintingArtId" />
        <input type="submit" value="Delete" class="btn btn-danger" /> |
        <a asp-page="./Index">Back to List</a>
    </form>
</div>

7.5.2.Delete.cshtml.cs
using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace OilPaintingArt_UyDev.Pages.OilPaintingArtPage
{
    public class DeleteModel : PageModel
    {
        private readonly IArtRepo _artRepo;

        public DeleteModel(IArtRepo artRepo)
        {
            _artRepo = artRepo;
        }

        [BindProperty]
        public OilPaintingArt OilPaintingArt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oilpaintingart = await _artRepo.GetOilPaintingArtById(id ?? default(int));

            if (oilpaintingart == null)
            {
                return NotFound();
            }
            else
            {
                OilPaintingArt = oilpaintingart;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                await _artRepo.DeletePainting(id ?? default(int));
                TempData["Message"] = "Delete Succesfull";

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
===================================================================
Validation-bos

full-bos

using System.ComponentModel.DataAnnotations;

namespace BOs
{
    public partial class OilPaintingArt
    {
        [Required(ErrorMessage = "Oil Painting Art Id is required")]
        public int OilPaintingArtId { get; set; }
        [Required(ErrorMessage = "Art Title is required")]
        [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Must begin capital letter")]
        public string ArtTitle { get; set; } = null!;
        [Required(ErrorMessage = "Art Description is required")]
        public string? OilPaintingArtLocation { get; set; }
        [Required(ErrorMessage = "Art Style is required")]
        public string? OilPaintingArtStyle { get; set; }
        [Required(ErrorMessage = "Art Dimension is required")]
        public string? Artist { get; set; }
        [Required(ErrorMessage = "Art Dimension is required")]
        public string? NotablFeatures { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal? PriceOfOilPaintingArt { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 50, ErrorMessage = "Quantity must be between 1 and 50")]
        public int? StoreQuantity { get; set; }
        [Required(ErrorMessage = "Created Date is required")]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Supplier Id is required")]
        public string? SupplierId { get; set; }

        public virtual SupplierCompany? Supplier { get; set; }
    }
}

some regex
Require: [Required(ErrorMessage = "Art Dimension is required")]
Range: [Range(1, 50, ErrorMessage = "Quantity must be between 1 and 50")]
Bat dau bang ky tu chu: [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Must begin capital letter")]




 */


//OPEN RAZOR PROJECT -> ADD CLIENTSIDE LIB -> JSDELIVR -> microsoft/signalr choose 2 js

//


//OPEN RAZOR PROJECT -> ADD CLIENTSIDE LIB -> JSDELIVR -> microsoft/signalr choose 2 js

//ADD TO INDEX WHERE RELOAD PAGE
//@section Scripts
//{
//    <script src="~/lib/microsoft/signalr/dist/browser/signalr.min.js"></script>
//    <script>
//        const connection = new signalR.HubConnectionBuilder()
//            .withUrl("/sinalRHub")
//            .build();

//connection.on("Change", function()
//{
//    location.reload();
//});

//connection.start().catch(function(err) {
//    console.error(err.toString());
//});
//    </ script >
//}

////MAKE A FOLDER AT PAGES (./Pages/Hubs/SignalRHub.cs)

//using Microsoft.AspNetCore.SignalR;
//using System.Threading.Tasks;

//namespace PharmaceuticalManagement_LaiChiThinh.Hubs
//{
//    public class SignalRHub : Hub
//    {
//        public async Task NotifyChange()
//        {
//            await Clients.All.SendAsync("Change");
//        }
//    }
//}

////PROGRAM.CS
//builder.Services.AddSignalR();
//app.MapHub<SignalRHub>("/sinalRHub");

////CALL SIGNALR
//private readonly IHubContext<SignalRHub> _hubContext;
//await _hubContext.Clients.All.SendAsync("Change");
