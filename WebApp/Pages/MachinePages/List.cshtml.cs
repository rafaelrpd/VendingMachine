using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
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

        public Machine machine { get; set; } = SeedData.Initialize();
        public void OnGet()
        {
        }
    }
}
