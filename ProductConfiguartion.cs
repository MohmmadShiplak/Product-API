using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Products_API
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Configuring primary key
            builder.HasKey(p => p.Id);

            // Configuring property
            builder.Property(p => p.Name)
                   .IsRequired() // Name is required
                   .HasMaxLength(100); // Max length of 100

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)"); // Ensuring the decimal precision for Price


            builder.ToTable("Products");


        }
    }
}
