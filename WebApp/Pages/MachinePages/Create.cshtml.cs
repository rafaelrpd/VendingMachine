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

        [BindProperty]
        public Machine FormMachine { get; set; } = default!;

        public RedirectToPageResult OnPost()
        {
            FormMachine.MachineId = SharedData.MachineList.LastOrDefault() == null ? 0 : SharedData.MachineList.Last().MachineId + 1;
            FormMachine.MachineProductList = new List<Product> { };
            SharedData.MachineList.Add(FormMachine);
            return RedirectToPage("./List");
        }
    }
}
