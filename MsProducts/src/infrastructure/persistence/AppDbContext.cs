using Microsoft.EntityFrameworkCore;
using MsProducts.Domain.Entities;

namespace MsProducts.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
    {
        // Nombre de la tabla
        entity.ToTable("Products");

        // Clave primaria
        entity.HasKey(e => e.Id);

        // Propiedades
        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(150);

        entity.Property(e => e.Description)
              .HasMaxLength(500);

        entity.Property(e => e.Category)
              .HasMaxLength(100);

        entity.Property(e => e.ImageUri)
              .HasMaxLength(300);

        entity.Property(e => e.Price)
              .HasColumnType("int"); // podrías usar decimal(18,2) si manejas centavos

        entity.Property(e => e.Stock)
              .HasDefaultValue(0); // valor por defecto

        entity.Property(e => e.CreatedAt)
              .HasColumnType("datetime2")
              .HasDefaultValueSql("GETUTCDATE()"); // valor default generado por SQL

        entity.Property(e => e.UpdatedAt)
              .HasColumnType("datetime2")
              .HasDefaultValueSql("GETUTCDATE()"); // inicial, luego lo manejas al actualizar

        // Índices
        entity.HasIndex(e => e.Name)
              .HasDatabaseName("IX_Products_Name");

        entity.HasIndex(e => e.Category)
              .HasDatabaseName("IX_Products_Category");
    });
    }
}
