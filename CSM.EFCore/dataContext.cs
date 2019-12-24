using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CSM.EFCore
{
    public partial class dataContext : DbContext
    {
        public dataContext()
        {
        }

        public dataContext(DbContextOptions<dataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<ImportExportHistory> ImportExportHistory { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceItemOrDiscount> InvoiceItemOrDiscount { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemDiscount> ItemDiscount { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=E:\\BigProjects\\CSM\\CSM.Xam\\CSM.Xam\\Files\\data.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).IsRequired();

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.DiscountName).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<ImportExportHistory>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkItemOrMaterial)
                    .IsRequired()
                    .HasColumnName("fk_ItemOrMaterial");

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.Reason).IsRequired();
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.CloseDate).IsRequired();

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.FkTable)
                    .IsRequired()
                    .HasColumnName("fk_Table");

                entity.Property(e => e.InvoiceNumber).IsRequired();
            });

            modelBuilder.Entity<InvoiceItemOrDiscount>(entity =>
            {
                entity.ToTable("Invoice_ItemOrDiscount");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkInvoice)
                    .IsRequired()
                    .HasColumnName("fk_Invoice");

                entity.Property(e => e.FkItemOrDiscount)
                    .IsRequired()
                    .HasColumnName("fk_ItemOrDiscount");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkCategory)
                    .IsRequired()
                    .HasColumnName("fk_Category");

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.ItemImage).IsRequired();

                entity.Property(e => e.ItemName).IsRequired();
            });

            modelBuilder.Entity<ItemDiscount>(entity =>
            {
                entity.ToTable("Item_Discount");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkDiscount)
                    .IsRequired()
                    .HasColumnName("fk_Discount");

                entity.Property(e => e.FkItem)
                    .IsRequired()
                    .HasColumnName("fk_Item");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.ImageIcon).IsRequired();

                entity.Property(e => e.MenuName).IsRequired();
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => new { e.FkMenu, e.FkItem });

                entity.ToTable("Menu_Item");

                entity.Property(e => e.FkMenu).HasColumnName("fkMenu");

                entity.Property(e => e.FkItem).HasColumnName("fkItem");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.FkZone)
                    .IsRequired()
                    .HasColumnName("fk_Zone");

                entity.Property(e => e.TableName).IsRequired();
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.ZoneName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
