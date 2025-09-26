using Microsoft.EntityFrameworkCore;
using MsTransactions.Domain.Entities;

namespace MsTransactions.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();
    public DbSet<TransactionDetailEntity> TransactionDetails => Set<TransactionDetailEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionEntity>(entity =>
        {
            // Nombre de la tabla
            entity.ToTable("Transactions");

            // Clave primaria
            entity.HasKey(e => e.Id);

            // Propiedades
            entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<int>(); // Almacenar como int en la base de datos

            entity.Property(e => e.TotalAmount)
                .HasColumnType("int");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);
                
            entity.Property(e => e.Comment)
                .HasColumnType("nvarchar(max)");   


            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // valor default generado por SQL

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // inicial, luego lo manejas al actualizar

            // Relación uno-a-muchos con TransactionDetailEntity
            entity.HasMany(e => e.Details)
                  .WithOne(d => d.Transaction)
                  .HasForeignKey(d => d.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade); // Si se elimina una transacción, eliminar sus detalles

            // Índices
            entity.HasIndex(e => e.Type)
                .HasDatabaseName("IX_Transactions_Type");

        });

        modelBuilder.Entity<TransactionDetailEntity>(entity =>
        {
            // Nombre de la tabla
            entity.ToTable("TransactionDetails");

            // Clave primaria
            entity.HasKey(e => e.Id);

            // Propiedades
            entity.Property(e => e.ProductId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnType("int");

            entity.Property(e => e.UnitPrice)
                .IsRequired()
                .HasColumnType("int");

            entity.Property(e => e.Total)
                .IsRequired()
                .HasColumnType("int");

            // Clave foránea a TransactionEntity
            entity.Property(e => e.TransactionId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // valor default generado por SQL

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()"); // inicial, luego lo manejas al actualizar

            // Índices
            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_TransactionDetails_ProductId");

            entity.HasIndex(e => e.TransactionId)
                .HasDatabaseName("IX_TransactionDetails_TransactionId");
        });
    }
}
