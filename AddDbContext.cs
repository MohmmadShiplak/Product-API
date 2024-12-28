using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace Products_API
{
    public class AddDbContext:DbContext
    {


        public AddDbContext _Context;


        public DbSet<Product> Products { get; set; }

        //public DbSet<ProductDetails>ProductDetails { get; set; }

        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options)
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
    .Build();

            var connectionString = config.GetSection("constr").Value;

            optionsBuilder.UseSqlServer(connectionString);



        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasConversion<decimal>();


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddDbContext).Assembly);
        }


 





    }
}
