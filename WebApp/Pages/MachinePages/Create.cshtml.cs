using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Pages.MachinePages
{
    public class CreateModel : PageModel
    {
        public void OnGet()
        {

        }

        public int ProductCount { get; set; } = 3;

        [BindProperty]
        public Machine FormMachine { get; set; } = default!;


        public RedirectToPageResult OnPost()
        {
            SharedData.MachineList.Add(FormMachine);
            SharedData.ProductList.AddRange(FormMachine.MachineProductList);
            return RedirectToPage("/MachinePages/List");
        }
    }
}
