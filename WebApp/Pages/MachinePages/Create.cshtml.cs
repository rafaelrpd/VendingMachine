using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;

namespace WebApp.Pages.MachinePages
{
    public class CreateModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public Machine FormMachine { get; set; } = new Machine();

        public RedirectToPageResult OnPost()
        {
            SharedData.MachineList.Add(FormMachine);
            return RedirectToPage("/MachinePages/List");
        }
    }
}
