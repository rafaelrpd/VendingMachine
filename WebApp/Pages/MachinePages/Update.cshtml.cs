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
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Machine FormMachine { get; set; }

        public IActionResult OnGet(Guid? Id)
        {
            if (Id == null || Machines == null)
            {
                return Redirect("/MachinePages/List");
            }

            var machineExist = Machines.FirstOrDefault(m => m.MachineId.Equals(Id));
            if (machineExist == null)
            {
                return Redirect("/MachinePages/List");
            }

            FormMachine = machineExist;
            return Page();
        }

        public IActionResult OnPost()
        {
            // TODO Solve problem with Guid from machine and product changing after each post.
            SharedData.MachineList.First(m => m.MachineId.Equals(Id)).MachineName = FormMachine.MachineName;
            SharedData.MachineList.First(m => m.MachineId.Equals(Id)).MachineBalance = FormMachine.MachineBalance;
            SharedData.MachineList.First(m => m.MachineId.Equals(Id)).MachineProductList = FormMachine.MachineProductList;
            SharedData.ProductList.AddRange(FormMachine.MachineProductList);
            return RedirectToPage("/MachinePages/List");
        }
    }
}
