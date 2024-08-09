using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.ViewModels;

namespace WebApp.Pages.Products
{
    public class ProductsModel : PageModel
    {
        public List<ProductViewModel> Products { get; set; }
        public void OnGet()
        {
            Products = new List<ProductViewModel>
            {
                new ProductViewModel { Name = "Product 1", Price = 1.00m, Quantity = 10 },
                new ProductViewModel { Name = "Product 2", Price = 2.50m, Quantity = 5 },
            };
        }
    }
}
