using KaercherNet.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KaercherNet.API
{
    public class KaercherNetDbContext : DbContext
    {
        public KaercherNetDbContext(DbContextOptions<KaercherNetDbContext> options) :base (options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
