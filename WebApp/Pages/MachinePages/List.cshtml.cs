using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Data.Shared;
using WebApp.Models.Machines;

namespace WebApp.Pages.MachinePages
{
    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> _logger;
        public ListModel(ILogger<ListModel> logger)
        {
            _logger = logger;
        }

        public List<Machine> Machine = SharedData.MachineList;
        public void OnGet()
        {
        }
    }
}
