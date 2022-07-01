using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;

namespace WebApp.Pages.MachinePages
{
    public class UpdateModel : PageModel
    {
        private readonly List<Machine> Machines = SharedData.MachineList;

        [BindProperty(SupportsGet = true)]
        public Machine FormMachine { get; set; }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null || Machines == null)
            {
                return Redirect("../List");
            }

            var machineExist = Machines.FirstOrDefault(m => m.MachineId.Equals(id));
            if (machineExist == null)
            {
                return Redirect("../List");
            }

            FormMachine = machineExist;
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/MachinePages/List");
        }
    }
}
