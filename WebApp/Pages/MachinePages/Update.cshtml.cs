using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Pages.MachinePages
{
    public class UpdateModel : PageModel
    {
        private readonly List<Machine> Machines = SharedData.MachineList;

        [BindProperty]
        public Machine FormMachine { get; set; } = default!;

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

        public IActionResult OnPost(Guid Id)
        {
            var _sharedDataMachineIndex = SharedData.MachineList.FindIndex(m => m.MachineId.Equals(Id));
            SharedData.MachineList[_sharedDataMachineIndex] = FormMachine;
            return RedirectToPage("/MachinePages/List");
        }

        public void OnPostAddProduct()
        {
            FormMachine.MachineProductList.Add(new Product());
        }

        public void OnPostRemoveProduct(int index)
        {
            FormMachine.MachineProductList.RemoveAt(index);
        }
    }
}
