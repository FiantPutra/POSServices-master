using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace POSServices.Models
{
    public partial class DB_BIENSI_POSContext : DbContext
    {
        public DB_BIENSI_POSContext()
        {
        }

        public DB_BIENSI_POSContext(DbContextOptions<DB_BIENSI_POSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationVersion> ApplicationVersion { get; set; }
        public virtual DbSet<AssHeadSecurityMatrix> AssHeadSecurityMatrix { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<CashierShift> CashierShift { get; set; }
        public virtual DbSet<ClosingStore> ClosingStore { get; set; }
        public virtual DbSet<CostCategory> CostCategory { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyDenomination> CurrencyDenomination { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerGroup> CustomerGroup { get; set; }
        public virtual DbSet<DeviceTable> DeviceTable { get; set; }
        public virtual DbSet<DiscountCategory> DiscountCategory { get; set; }
        public virtual DbSet<DiscountItemSelected> DiscountItemSelected { get; set; }
        public virtual DbSet<DiscountRetail> DiscountRetail { get; set; }
        public virtual DbSet<DiscountRetailLines> DiscountRetailLines { get; set; }
        public virtual DbSet<DiscountSetup> DiscountSetup { get; set; }
        public virtual DbSet<DiscountSetupLines> DiscountSetupLines { get; set; }
        public virtual DbSet<DiscountSetupStore> DiscountSetupStore { get; set; }
        public virtual DbSet<DiscountStore> DiscountStore { get; set; }
        public virtual DbSet<DoConfirm> DoConfirm { get; set; }
        public virtual DbSet<DoPending> DoPending { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeePossition> EmployeePossition { get; set; }
        public virtual DbSet<EmployeeTarget> EmployeeTarget { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseStore> ExpenseStore { get; set; }
        public virtual DbSet<HotransactionType> HotransactionType { get; set; }
        public virtual DbSet<InventoryLines> InventoryLines { get; set; }
        public virtual DbSet<InventoryTransaction> InventoryTransaction { get; set; }
        public virtual DbSet<InventoryTransactionLines> InventoryTransactionLines { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemDimensionBrand> ItemDimensionBrand { get; set; }
        public virtual DbSet<ItemDimensionColor> ItemDimensionColor { get; set; }
        public virtual DbSet<ItemDimensionDepartment> ItemDimensionDepartment { get; set; }
        public virtual DbSet<ItemDimensionDepartmentType> ItemDimensionDepartmentType { get; set; }
        public virtual DbSet<ItemDimensionGender> ItemDimensionGender { get; set; }
        public virtual DbSet<ItemDimensionSize> ItemDimensionSize { get; set; }
        public virtual DbSet<ItemGroup> ItemGroup { get; set; }
        public virtual DbSet<LogRecord> LogRecord { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<LoginStore> LoginStore { get; set; }
        public virtual DbSet<LoginWeb> LoginWeb { get; set; }
        public virtual DbSet<MutasiApproverMatrix> MutasiApproverMatrix { get; set; }
        public virtual DbSet<PriceList> PriceList { get; set; }
        public virtual DbSet<SequenceNumberLog> SequenceNumberLog { get; set; }
        public virtual DbSet<StockTake> StockTake { get; set; }
        public virtual DbSet<StockTakeLine> StockTakeLine { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreDiscount> StoreDiscount { get; set; }
        public virtual DbSet<StoreMargin> StoreMargin { get; set; }
        public virtual DbSet<StorePaymentMethod> StorePaymentMethod { get; set; }
        public virtual DbSet<StoreTarget> StoreTarget { get; set; }
        public virtual DbSet<StoreType> StoreType { get; set; }
        public virtual DbSet<TempDofixing> TempDofixing { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionByPassx> TransactionByPassx { get; set; }
        public virtual DbSet<TransactionCopyx> TransactionCopyx { get; set; }
        public virtual DbSet<TransactionLines> TransactionLines { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }

        // Unable to generate entity type for table 'dbo.DATA'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CUOR_ITRN_MISS'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CUNO'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Department'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DiscountAPI'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DiscountItemAPI'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.rpt_vInventoryLines'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.rpt_Security'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=Server-VSU;Database=DB_BIENSI_POS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ApplicationVersion>(entity =>
            {
                entity.Property(e => e.AppType)
                    .HasColumnName("appType")
                    .HasMaxLength(50);

                entity.Property(e => e.ReasonUpdate).HasColumnName("reason_update");

                entity.Property(e => e.TanggalUpdate)
                    .HasColumnName("tanggal_update")
                    .HasColumnType("date");

                entity.Property(e => e.UrlDownload).HasColumnName("url_download");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<AssHeadSecurityMatrix>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AssEmployeId)
                    .IsRequired()
                    .HasColumnName("AssEmployeID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasKey(e => e.BankId)
                    .HasName("PK__Bank__AA08CB13E2924EE3");

                entity.Property(e => e.BankId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.BrandCode);

                entity.Property(e => e.BrandCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.JournalNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("date");
            });

            modelBuilder.Entity<CashierShift>(entity =>
            {
                entity.Property(e => e.CashierShiftId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingBalance).HasColumnType("money");

                entity.Property(e => e.ClosingTime).HasColumnType("datetime");

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OpeningBalance).HasColumnType("money");

                entity.Property(e => e.OpeningTime).HasColumnType("datetime");

                entity.Property(e => e.ShiftCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClosingStore>(entity =>
            {
                entity.Property(e => e.ClosignTranBal).HasColumnType("money");

                entity.Property(e => e.ClosingDeposit).HasColumnType("money");

                entity.Property(e => e.ClosingPettyCash).HasColumnType("money");

                entity.Property(e => e.ClosingStoreId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisputeDeposit).HasColumnType("money");

                entity.Property(e => e.DisputePettyCash).HasColumnType("money");

                entity.Property(e => e.DisputeTransBal).HasColumnType("money");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OpeningDeposit).HasColumnType("money");

                entity.Property(e => e.OpeningPettyCash).HasColumnType("money");

                entity.Property(e => e.OpeningTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.OpeningTransBal).HasColumnType("money");

                entity.Property(e => e.RealDeposit).HasColumnType("money");

                entity.Property(e => e.RealPettyCash).HasColumnType("money");

                entity.Property(e => e.RealTransBal).HasColumnType("money");

                entity.Property(e => e.StatusClose)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CostCategory>(entity =>
            {
                entity.Property(e => e.Coa)
                    .HasColumnName("COA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CostCategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CurrencyDenomination>(entity =>
            {
                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nominal).HasColumnType("money");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultCurr)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CustGroup)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CustGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_ToCustomerGroup");
            });

            modelBuilder.Entity<CustomerGroup>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<DeviceTable>(entity =>
            {
                entity.Property(e => e.DeviceId)
                    .HasColumnName("Device_ID")
                    .HasMaxLength(100);

                entity.Property(e => e.FirstLogin).HasMaxLength(50);

                entity.Property(e => e.InitialId)
                    .HasColumnName("Initial_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasMaxLength(50);

                entity.Property(e => e.TypeDevice)
                    .HasColumnName("Type_Device")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DiscountCategory>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DiscountItemSelected>(entity =>
            {
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.DiscountItemSelected)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountItemSelected_ToItem");
            });

            modelBuilder.Entity<DiscountRetail>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.AmountMax).HasColumnType("money");

                entity.Property(e => e.AmountMin).HasColumnType("money");

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountCash).HasColumnType("money");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountPartner)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CustomerGroup)
                    .WithMany(p => p.DiscountRetail)
                    .HasForeignKey(d => d.CustomerGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountRetail_ToTable");
            });

            modelBuilder.Entity<DiscountRetailLines>(entity =>
            {
                entity.HasIndex(e => e.DiscountRetailId)
                    .HasName("NonClusteredIndex-20191229-210428");

                entity.Property(e => e.AmountMax).HasColumnType("money");

                entity.Property(e => e.AmountMin).HasColumnType("money");

                entity.Property(e => e.AmountTransaction)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CashDiscount).HasColumnType("money");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DiscountPrecentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Qty).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.DiscountRetailLinesArticle)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_DiscountRetailLines_ToItem");

                entity.HasOne(d => d.ArticleIdDiscountNavigation)
                    .WithMany(p => p.DiscountRetailLinesArticleIdDiscountNavigation)
                    .HasForeignKey(d => d.ArticleIdDiscount)
                    .HasConstraintName("FK_DiscountRetailLines_ToItemDiscount");

                entity.HasOne(d => d.BrandCodeNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.BrandCode)
                    .HasConstraintName("FK_DiscountRetailLines_ToBranD");

                entity.HasOne(d => d.ColorNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.Color)
                    .HasConstraintName("FK_DiscountRetailLines_ToColor");

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.Department)
                    .HasConstraintName("FK_DiscountRetailLines_ToDepartment");

                entity.HasOne(d => d.DepartmentTypeNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.DepartmentType)
                    .HasConstraintName("FK_DiscountRetailLines_ToDepartmentType");

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.Gender)
                    .HasConstraintName("FK_DiscountRetailLines_ToGender");

                entity.HasOne(d => d.SizeNavigation)
                    .WithMany(p => p.DiscountRetailLines)
                    .HasForeignKey(d => d.Size)
                    .HasConstraintName("FK_DiscountRetailLines_ToSize");
            });

            modelBuilder.Entity<DiscountSetup>(entity =>
            {
                entity.Property(e => e.AmountMax).HasColumnType("money");

                entity.Property(e => e.AmountMin).HasColumnType("money");

                entity.Property(e => e.ApplytoAllstore).HasDefaultValueSql("((0))");

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DiscountCash).HasColumnType("money");

                entity.Property(e => e.DiscountCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiscountSetupLines>(entity =>
            {
                entity.Property(e => e.AmountMax).HasColumnType("money");

                entity.Property(e => e.AmountMin).HasColumnType("money");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountCash).HasColumnType("money");

                entity.Property(e => e.DiscountPrecentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.DiscountSetup)
                    .WithMany(p => p.DiscountSetupLines)
                    .HasForeignKey(d => d.DiscountSetupId)
                    .HasConstraintName("FK_DiscountSetupLines_DiscountSetup");
            });

            modelBuilder.Entity<DiscountSetupStore>(entity =>
            {
                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DiscountSetupStore)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountStore_ToStore1");
            });

            modelBuilder.Entity<DiscountStore>(entity =>
            {
                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DiscountStore)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountStore_ToStore");
            });

            modelBuilder.Entity<DoConfirm>(entity =>
            {
                entity.ToTable("DO_CONFIRM");

                entity.Property(e => e.CheckDll)
                    .HasColumnName("CheckDLL")
                    .HasMaxLength(50);

                entity.Property(e => e.DoNumber)
                    .HasColumnName("DO_Number")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.WarehouseDestination).HasMaxLength(50);

                entity.Property(e => e.WarehouseOrigin).HasMaxLength(50);
            });

            modelBuilder.Entity<DoPending>(entity =>
            {
                entity.ToTable("DO_PENDING");

                entity.Property(e => e.CheckDll)
                    .HasColumnName("CheckDLL")
                    .HasMaxLength(50);

                entity.Property(e => e.DoNumber)
                    .HasColumnName("DO_Number")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.WarehouseDestination).HasMaxLength(50);

                entity.Property(e => e.WarehouseOrigin).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdateDate).HasColumnType("date");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.HasOne(d => d.Possition)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PossitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_ToEmployeePossition");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_ToStore");
            });

            modelBuilder.Entity<EmployeePossition>(entity =>
            {
                entity.Property(e => e.CanConfirmDo).HasColumnName("CanConfirmDO");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PossitionId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeTarget>(entity =>
            {
                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Periode).HasColumnType("date");

                entity.Property(e => e.Target).HasColumnType("money");
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CostCategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CostCategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Itrn)
                    .HasColumnName("ITRN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Orno)
                    .HasColumnName("ORNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExpenseStore>(entity =>
            {
                entity.Property(e => e.RemaingBudget).HasColumnType("money");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalBudget).HasColumnType("money");

                entity.Property(e => e.TotalExpense).HasColumnType("money");
            });

            modelBuilder.Entity<HotransactionType>(entity =>
            {
                entity.ToTable("HOTransactionType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WarehouseFrom)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.WarehouseTo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InventoryLines>(entity =>
            {
                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.WarehouseId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InventoryLines)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryLines_Toitem");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.InventoryLines)
                    .HasForeignKey(d => d.WarehouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryLines_ToWarehouse");
            });

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasIndex(e => e.TransactionId)
                    .HasName("IX_InventoryTransaction")
                    .IsUnique();

                entity.Property(e => e.DeliveryOrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeNameToApprove)
                    .HasColumnName("employeeNameToApprove")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeToApprove)
                    .HasColumnName("employeeToApprove")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsSyncToInfor).HasColumnName("isSyncToInfor");

                entity.Property(e => e.MutasiType).HasColumnName("mutasiType");

                entity.Property(e => e.MutasiTypeName)
                    .HasColumnName("mutasiTypeName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reasons)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RequestDeliveryDate).HasColumnType("date");

                entity.Property(e => e.Sjlama)
                    .HasColumnName("SJlama")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SyncDate).HasColumnType("datetime");

                entity.Property(e => e.TotalAmount).HasColumnType("money");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Urridn)
                    .HasColumnName("URRIDN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WarehouseDestination)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WarehouseOriginal)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InventoryTransactionLines>(entity =>
            {
                entity.Property(e => e.ArticleId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PackingNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Urdlix).HasColumnName("URDLIX");

                entity.Property(e => e.Urridl).HasColumnName("URRIDL");

                entity.Property(e => e.Urridn)
                    .HasColumnName("URRIDN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValueSalesPrice).HasColumnType("money");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(e => e.ItemId)
                    .HasName("NonClusteredIndex-20200121-094316");

                entity.Property(e => e.BrandId).HasColumnName("Brand_id");

                entity.Property(e => e.ColorId).HasColumnName("Color_id");

                entity.Property(e => e.CreateDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepartmentId).HasColumnName("Department_id");

                entity.Property(e => e.DepartmentTypeId).HasColumnName("DepartmentType_id");

                entity.Property(e => e.GenderId).HasColumnName("Gender_id");

                entity.Property(e => e.IsServiceItem)
                    .HasColumnName("isServiceItem")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ItemGroup)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ItemIdAlias)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SizeId).HasColumnName("Size_id");
            });

            modelBuilder.Entity<ItemDimensionBrand>(entity =>
            {
                entity.ToTable("ItemDimension_Brand");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemDimensionColor>(entity =>
            {
                entity.ToTable("ItemDimension_Color");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemDimensionDepartment>(entity =>
            {
                entity.ToTable("ItemDimension_Department");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemDimensionDepartmentType>(entity =>
            {
                entity.ToTable("ItemDimension_DepartmentType");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemDimensionGender>(entity =>
            {
                entity.ToTable("ItemDimension_Gender");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemDimensionSize>(entity =>
            {
                entity.ToTable("ItemDimension_Size");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ItemGroup>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogRecord>(entity =>
            {
                entity.HasIndex(e => e.TransactionId)
                    .HasName("idx_TransactionId_notnull")
                    .IsUnique()
                    .HasFilter("([TransactionId] IS NOT NULL)");

                entity.Property(e => e.Tag).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.TransactionId).HasMaxLength(50);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(25);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<LoginStore>(entity =>
            {
                entity.Property(e => e.StoreCode).HasMaxLength(10);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.LoginStore)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LoginStore_ToStore");
            });

            modelBuilder.Entity<LoginWeb>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.LoginWeb)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_LoginWeb_ToEmployee");
            });

            modelBuilder.Entity<MutasiApproverMatrix>(entity =>
            {
                entity.HasKey(e => e.StoreMatrix);

                entity.Property(e => e.StoreMatrix).ValueGeneratedNever();

                entity.Property(e => e.ApproverCity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverNational)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverRegional)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.StoreMatrixNavigation)
                    .WithOne(p => p.MutasiApproverMatrix)
                    .HasForeignKey<MutasiApproverMatrix>(d => d.StoreMatrix)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MutasiApproverMatrix_MutasiApproverMatrix");
            });

            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.HasIndex(e => e.ItemId)
                    .HasName("Index_ItemId")
                    .IsUnique();

                entity.Property(e => e.Currency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalesPrice).HasColumnType("money");
            });

            modelBuilder.Entity<SequenceNumberLog>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.LastNumberSequence).HasMaxLength(50);

                entity.Property(e => e.LastTransId).HasMaxLength(50);

                entity.Property(e => e.StoreCode).HasMaxLength(50);

                entity.Property(e => e.TransactionType).HasMaxLength(50);
            });

            modelBuilder.Entity<StockTake>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnType("date");
            });

            modelBuilder.Entity<StockTakeLine>(entity =>
            {
                entity.Property(e => e.ArticleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOpen).HasColumnType("date");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileStore).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Regional)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StoreTypeId).HasDefaultValueSql("((0))");

                entity.Property(e => e.TargetValue).HasColumnType("money");

                entity.Property(e => e.WarehouseId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StoreDiscount>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreDiscount)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StoreDiscount_ToTable");
            });

            modelBuilder.Entity<StoreMargin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StorePaymentMethod>(entity =>
            {
                entity.HasIndex(e => e.Detailid)
                    .HasName("IX_StorePaymentMethod");

                entity.Property(e => e.BankCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Detailid)
                    .IsRequired()
                    .HasColumnName("detailid")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BankCodeNavigation)
                    .WithMany(p => p.StorePaymentMethod)
                    .HasForeignKey(d => d.BankCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorePaymentMethod_ToBank");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StorePaymentMethod)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StorePaymentMethod_ToStore");
            });

            modelBuilder.Entity<StoreTarget>(entity =>
            {
                entity.Property(e => e.Periode).HasColumnType("date");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Target).HasColumnType("money");
            });

            modelBuilder.Entity<StoreType>(entity =>
            {
                entity.Property(e => e.InforOrderTypeNormal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InforOrderTypeRetur)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InforXrcdnormal)
                    .HasColumnName("InforXRCDNormal")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InforXrcdretur)
                    .HasColumnName("InforXRCDRetur")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempDofixing>(entity =>
            {
                entity.HasKey(e => e.Oqdlix);

                entity.ToTable("Temp_DOFixing");

                entity.Property(e => e.Oqdlix)
                    .HasColumnName("OQDLIX")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.TransactionId, e.StoreCode, e.EmployeeCode })
                    .HasName("t1");

                entity.Property(e => e.Bank1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Bank2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cash).HasColumnType("money");

                entity.Property(e => e.Change).HasColumnType("money");

                entity.Property(e => e.ClosingShiftId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingStoreId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edc1).HasColumnType("money");

                entity.Property(e => e.Edc2).HasColumnType("money");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.InforSyncDate).HasColumnType("date");

                entity.Property(e => e.IsSyncInfor).HasColumnName("isSyncInfor");

                entity.Property(e => e.MarginTransaction).HasColumnType("money");

                entity.Property(e => e.MethodOfPayment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoRef1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoRef2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Orno)
                    .HasColumnName("ORNO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecieptCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftCode).HasMaxLength(10);

                entity.Property(e => e.Spgid).HasColumnName("SPGId");

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Text1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Text2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Text3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TotalAmounTransaction)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalDiscount)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TransDateStore).HasColumnType("datetime");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Voucher).HasColumnType("money");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionByPassx>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionCopyx>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.TransactionId, e.StoreCode, e.EmployeeCode })
                    .HasName("t1");

                entity.Property(e => e.Bank1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Bank2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cash).HasColumnType("money");

                entity.Property(e => e.Change).HasColumnType("money");

                entity.Property(e => e.ClosingShiftId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingStoreId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edc1).HasColumnType("money");

                entity.Property(e => e.Edc2).HasColumnType("money");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MarginTransaction).HasColumnType("money");

                entity.Property(e => e.MethodOfPayment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoRef1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoRef2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Orno)
                    .HasColumnName("ORNO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecieptCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShiftCode).HasMaxLength(10);

                entity.Property(e => e.Spgid).HasColumnName("SPGId");

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Text1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Text2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Text3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TotalAmounTransaction).HasColumnType("money");

                entity.Property(e => e.TotalDiscount).HasColumnType("money");

                entity.Property(e => e.TransDateStore).HasColumnType("datetime");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Voucher).HasColumnType("money");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionLines>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.ArticleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleIdAlias)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ArticleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Spgid)
                    .HasColumnName("SPGID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.ConfirmPassword).HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NewPassword).HasMaxLength(50);

                entity.Property(e => e.OldPassword).HasMaxLength(50);

                entity.Property(e => e.Role).HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLogin_ToEmployee");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value).HasColumnType("money");

                entity.Property(e => e.VoucherCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Warehous__A25C5AA67E5BA08C");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Division)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Regional)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
