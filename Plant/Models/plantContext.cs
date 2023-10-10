using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Plant.Models
{
    public partial class plantContext : DbContext
    {
       

        public plantContext(DbContextOptions<plantContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogCategory> BlogCategories { get; set; }
        public virtual DbSet<BlogTranslation> BlogTranslations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryNewTranslation> CategoryNewTranslations { get; set; }
        public virtual DbSet<CategoryNews> CategoryNews { get; set; }
        public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        public virtual DbSet<ProductImg> ProductImgs { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.BlogImg).IsRequired();
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogCategories)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogCategories_Blogs");

                entity.HasOne(d => d.CategoryNew)
                    .WithMany(p => p.BlogCategories)
                    .HasForeignKey(d => d.CategoryNewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogCategories_CategoryNews");
            });

            modelBuilder.Entity<BlogTranslation>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogTranslations)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogTranslations_Blogs");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.BlogTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogTranslations_Languages");
            });

            modelBuilder.Entity<CategoryNewTranslation>(entity =>
            {
                entity.Property(e => e.CategoryNewName).IsRequired();

                entity.HasOne(d => d.CategoryNew)
                    .WithMany(p => p.CategoryNewTranslations)
                    .HasForeignKey(d => d.CategoryNewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryNewTranslations_CategoryNews");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.CategoryNewTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryNewTranslations_Languages");
            });

            modelBuilder.Entity<CategoryNews>(entity =>
            {
                entity.HasKey(e => e.CategoryNewId);
            });

            modelBuilder.Entity<CategoryTranslation>(entity =>
            {
                entity.Property(e => e.CategoryName).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryTranslations)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryTranslations_Categories");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.CategoryTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryTranslations_Languages");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.District).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Road).IsRequired();

                entity.Property(e => e.Ward).IsRequired();
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.FeedbackContent).IsRequired();
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.LangId);

                entity.Property(e => e.LangName).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Money).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OrderStatus).IsRequired();

                entity.Property(e => e.ShipFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Feedback)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK_Orders_Feedbacks");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Image).IsRequired();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCategories_Categories");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCategories_Products");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.Property(e => e.Color).IsRequired();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductColors_Products");
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.Property(e => e.Img).IsRequired();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImgs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImgs_Products");
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrders_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrders_Products");
            });

            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductName).IsRequired();

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.ProductTranslations)
                    .HasForeignKey(d => d.LangId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTranslations_Languages");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTranslations)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTranslations_Products");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
