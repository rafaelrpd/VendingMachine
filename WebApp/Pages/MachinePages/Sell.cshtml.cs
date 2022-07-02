using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data.Shared;
using WebApp.Models.Machines;

namespace WebApp.Pages.MachinePages
{
    public class SellModel : PageModel
    {
        public List<Machine> Machine = SharedData.MachineList;

        [BindProperty]
        public List<string> Error { get; set; } = default!;

        [BindProperty]
        public decimal MoneyToAdd { get; set; } = decimal.Zero;

        public void OnGet()
        {

        }

        public IActionResult OnPostAddMoney(Guid Id)
        {
            if (MoneyToAdd <= 0)
            {
                Error = new List<string>();
                Error.Add("Balance error");
                Error.Add("Please, add more money. Value should be positive.");
                return Page();
            }
            Machine.First(m => m.MachineId.Equals(Id)).MachineBalance += MoneyToAdd;
            return RedirectToPagePermanent("Sell");
        }
        public IActionResult OnPostSellItem(int MachineIndex, int ProductIndex)
        {
            var _machineBalance = Machine[MachineIndex].MachineBalance;
            var _productPrice = Machine[MachineIndex].MachineProductList[ProductIndex].ProductPrice;
            var _productQuantity = Machine[MachineIndex].MachineProductList[ProductIndex].ProductQuantity;
            if (_productQuantity > 0 && (_machineBalance - _productPrice) >= 0)
            {
                Machine[MachineIndex].MachineProductList[ProductIndex].ProductQuantity -= 1;
                Machine[MachineIndex].MachineBalance -= _productPrice;
            }
            else if (_productQuantity <= 0)
            {
                Error = new List<string>();
                Error.Add("Quantity error");
                Error.Add("Product without disponibility. Add more products or buy something else.");
                return Page();
            }
            else if ((_machineBalance - _productPrice) <= 0)
            {
                Error = new List<string>();
                Error.Add("Balance error");
                Error.Add("Please, add more money.");
                return Page();
            }
            return RedirectToPagePermanent("Sell");
        }
    }
}