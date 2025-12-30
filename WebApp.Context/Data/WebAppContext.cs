using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Product { get; set; } = default!;
    }
}
