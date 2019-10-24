using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CSM.EFCore
{
    public partial class dataContext : DbContext
    {
        private DbContextOptionsBuilder<dataContext> _optionBuilder;
        public dataContext(string connectionString) : base(
            SqliteDbContextOptionsBuilderExtensions
                .UseSqlite(new DbContextOptionsBuilder(), connectionString)
                .Options
            )
        {
        }

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
        public virtual DbSet<InvoiceItem> InvoiceItem { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemItemOption> ItemItemOption { get; set; }
        public virtual DbSet<ItemOption> ItemOption { get; set; }
        public virtual DbSet<ItemPrice> ItemPrice { get; set; }
        public virtual DbSet<ItemPriceMaterial> ItemPriceMaterial { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<WeightUnit> WeightUnit { get; set; }
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
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("Invoice_Item");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkInvoice)
                    .IsRequired()
                    .HasColumnName("fk_Invoice");

                entity.Property(e => e.FkItem)
                    .IsRequired()
                    .HasColumnName("fk_Item");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkCategory).HasColumnName("fk_Category");

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.FkWeightUnit).HasColumnName("fk_WeightUnit");

                entity.Property(e => e.ItemImage).IsRequired();

                entity.Property(e => e.ItemName).IsRequired();
            });

            modelBuilder.Entity<ItemItemOption>(entity =>
            {
                entity.ToTable("Item_ItemOption");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkItem)
                    .IsRequired()
                    .HasColumnName("fk_Item");

                entity.Property(e => e.FkItemOption)
                    .IsRequired()
                    .HasColumnName("fk_ItemOption");
            });

            modelBuilder.Entity<ItemOption>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.OptionName).IsRequired();
            });

            modelBuilder.Entity<ItemPrice>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkItem)
                    .IsRequired()
                    .HasColumnName("fk_Item");

                entity.Property(e => e.PriceName).IsRequired();
            });

            modelBuilder.Entity<ItemPriceMaterial>(entity =>
            {
                entity.ToTable("ItemPrice_Material");

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkItemPrice)
                    .IsRequired()
                    .HasColumnName("fk_ItemPrice");

                entity.Property(e => e.FkMaterial)
                    .IsRequired()
                    .HasColumnName("fk_Material");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.FkWeightUnit)
                    .IsRequired()
                    .HasColumnName("fk_WeightUnit");

                entity.Property(e => e.MaterialName).IsRequired();
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

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(e => e.KeySetting);

                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.SettingValue).IsRequired();
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

            modelBuilder.Entity<WeightUnit>(entity =>
            {
                entity.Property(e => e.CreationDate).IsRequired();

                entity.Property(e => e.Creator).IsRequired();

                entity.Property(e => e.FkStore)
                    .IsRequired()
                    .HasColumnName("fk_Store");

                entity.Property(e => e.IsDeleted).IsRequired();

                entity.Property(e => e.UnitName).IsRequired();
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
