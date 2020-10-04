
using Del.Common.Entities;
using Delevery.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Delevery.Web.Data
{
    public class DataContext : IdentityDbContext<Entities.User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<Category>()
            .HasIndex(t => t.Name)
            .IsUnique();

            modelBuilder.Entity<Product>()
           .HasIndex(t => t.Name)
           .IsUnique();


            modelBuilder.Entity<Restaurant>()
                .HasIndex(t => t.Address)
                .IsUnique();

            modelBuilder.Entity<Restaurant>()
                .HasIndex(t => t.Name)
                .IsUnique();

            

        }

    }
}
