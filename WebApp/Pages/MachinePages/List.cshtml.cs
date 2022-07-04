using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;

namespace WebApp.Pages.MachinePages
{
    public class ListModel : PageModel
    {
        private readonly List<Machine> Machines = SharedData.MachineList;
        private readonly ILogger<ListModel> _logger;
        public ListModel(ILogger<ListModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public List<Machine> FormMachinesList { get; set; }

        public IActionResult OnGet()
        {
            FormMachinesList = Machines;
            return Page();
        }

        public void OnPostDelete(int index)
        {
            FormMachinesList.RemoveAt(index);
            SharedData.MachineList = FormMachinesList;
        }
    }
}
