using Microsoft.EntityFrameworkCore;
using MsAuth.Domain.Entities;

namespace MsAuth.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
    {
        // Nombre de la tabla
        entity.ToTable("Users");

        // Clave primaria
        entity.HasKey(e => e.Id);

        // Propiedades

        entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(256);

        entity.Property(e => e.Password)
              .IsRequired()
              .HasMaxLength(512);
        entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);


        entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // valor default generado por SQL

        entity.Property(e => e.UpdatedAt)
              .HasColumnType("datetime2")
              .HasDefaultValueSql("GETUTCDATE()"); // inicial, luego lo manejas al actualizar

        // Índices
        entity.HasIndex(e => e.Email)
              .HasDatabaseName("IX_Users_Email");

    });
    }
}
