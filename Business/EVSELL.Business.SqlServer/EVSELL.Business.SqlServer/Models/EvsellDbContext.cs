using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Evsell.Business.SqlServer.Models;

public partial class EvsellDbContext : DbContext
{
    public EvsellDbContext()
    {
    }

    public EvsellDbContext(DbContextOptions<EvsellDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<BasketLog> BasketLogs { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<EnumUserType> EnumUserTypes { get; set; }

    public virtual DbSet<HtmlEmailPage> HtmlEmailPages { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceProduct> InvoiceProducts { get; set; }

    public virtual DbSet<InvoiceStatusLog> InvoiceStatusLogs { get; set; }

    public virtual DbSet<LogHttp> LogHttps { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductComment> ProductComments { get; set; }

    public virtual DbSet<ProductStockLog> ProductStockLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=evsellDb;Integrated Security=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.ToTable("Basket");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Basket_Product");
        });

        modelBuilder.Entity<BasketLog>(entity =>
        {
            entity.ToTable("BasketLog");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.HasIndex(e => e.Name, "IX_Company").IsUnique();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Companies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_User");
        });

        modelBuilder.Entity<EnumUserType>(entity =>
        {
            entity.ToTable("EnumUserType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<HtmlEmailPage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Page)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.CancelDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.GrandTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");
            entity.Property(e => e.VatTotal).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<InvoiceProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InvoiceProducts");

            entity.ToTable("InvoiceProduct");

            entity.Property(e => e.LineTotal)
                .HasComment("Tax+Total (Last)")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax)
                .HasComment("Total * 100 / Product.Vat (Second)")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total)
                .HasComment("Product.Price * Qty (Firts)")
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceProducts)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceProduct_Invoice");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceProduct_Product");
        });

        modelBuilder.Entity<InvoiceStatusLog>(entity =>
        {
            entity.ToTable("InvoiceStatusLog");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceStatusLogs)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceStatusLog_Invoice");
        });

        modelBuilder.Entity<LogHttp>(entity =>
        {
            entity.ToTable("LogHttp");

            entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
            entity.Property(e => e.RequestDateTime).HasColumnType("datetime");
            entity.Property(e => e.RequestRaw).IsRequired();
            entity.Property(e => e.ResponseDateTime).HasColumnType("datetime");
            entity.Property(e => e.ResponseRaw).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Image)
                .IsRequired()
                .HasMaxLength(2048);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");

            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Company");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<ProductComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Product_Comments");

            entity.ToTable("ProductComment");

            entity.Property(e => e.Comments)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Header)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductComments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductComment_Product");

            entity.HasOne(d => d.User).WithMany(p => p.ProductComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductComment_User");
        });

        modelBuilder.Entity<ProductStockLog>(entity =>
        {
            entity.ToTable("ProductStockLog");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.UserName, "IX_User").IsUnique();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FirtsName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_EnumUserType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
