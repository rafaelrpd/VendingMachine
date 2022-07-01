using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;
using System.Linq;

namespace WebApp.Pages.MachinePages
{
    public class UpdateModel : PageModel
    {
        private readonly List<Machine> Machines = SharedData.MachineList;

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
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
            var updatedProducts = 
                from updateProduct in FormMachine.MachineProductList
                from sharedProduct in SharedData.ProductList
                where updateProduct.ProductId == sharedProduct.ProductId
                select updateProduct;

            foreach (var product in updatedProducts)
            {
                SharedData.ProductList.First(p => p.ProductId.Equals(product.ProductId)).ProductName = product.ProductName;
                SharedData.ProductList.First(p => p.ProductId.Equals(product.ProductId)).ProductPrice = product.ProductPrice;
                SharedData.ProductList.First(p => p.ProductId.Equals(product.ProductId)).ProductQuantity = product.ProductQuantity;

            }
            return RedirectToPage("/MachinePages/List");
        }
    }
}
