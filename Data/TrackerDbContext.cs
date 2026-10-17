using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

namespace Tracker.Data;

public class TrackerDbContext : DbContext
{
    public TrackerDbContext()
    {
    }

    public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            Env.Load();

            string connectionString =
                Environment.GetEnvironmentVariable("TRACKER_CONNECTION_STRING")
                ?? throw new InvalidOperationException(
                    "TRACKER_CONNECTION_STRING was not found.");

            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Merchant> Merchants => Set<Merchant>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();
    public DbSet<User> Users => Set<User>();
    public DbSet<SavedCollection> SavedCollections => Set<SavedCollection>();
    public DbSet<SavedCollectionItem> SavedCollectionItems => Set<SavedCollectionItem>();
    public DbSet<ShareLink> ShareLinks => Set<ShareLink>();
    public DbSet<ShareLinkClick> ShareLinkClicks => Set<ShareLinkClick>();
    public DbSet<AffiliateConversion> AffiliateConversions => Set<AffiliateConversion>();
    public DbSet<PriceAlert> PriceAlerts => Set<PriceAlert>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CrawlRun> CrawlRuns => Set<CrawlRun>();
    public DbSet<CrawledListing> CrawledListings => Set<CrawledListing>();
    public DbSet<SponsoredListing> SponsoredListings => Set<SponsoredListing>();
    public DbSet<Status> Statuses => Set<Status>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Replace<ForeignKeyIndexConvention>(services =>
            new CatalogForeignKeyIndexConvention(
                services.GetRequiredService<ProviderConventionSetBuilderDependencies>()));
    }

    private sealed class CatalogForeignKeyIndexConvention(
        ProviderConventionSetBuilderDependencies dependencies) : ForeignKeyIndexConvention(dependencies)
    {
        protected override IConventionIndex? CreateIndex(
            IReadOnlyList<IConventionProperty> properties,
            bool unique,
            IConventionEntityTypeBuilder entityTypeBuilder)
        {
            // Users are looked up by ID/email, not by locale. Language is reference data.
            // Keep normal FK index conventions for every other relationship.
            if (!unique
                && entityTypeBuilder.Metadata.ClrType == typeof(User)
                && properties.Count == 1
                && properties[0].Name == nameof(User.PreferredLanguageCode))
            {
                return null;
            }

            return base.CreateIndex(properties, unique, entityTypeBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>(e =>
        {
            e.ToTable("Language", t =>
            {
                t.HasCheckConstraint(
                    "CK_Language_NativeName",
                    "length(trim(native_name)) > 0");
            });

            e.HasKey(x => x.LanguageCode).HasName("PK_Language");

            e.Property(x => x.LanguageCode)
                .HasColumnName("language_code")
                .HasMaxLength(10)
                .IsRequired();

            e.Property(x => x.NativeName)
                .HasColumnName("native_name")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.HasData(
                new { LanguageCode = "en", NativeName = "English", IsActive = true },
                new { LanguageCode = "ar", NativeName = "العربية", IsActive = true });
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Category", t =>
            {
                t.HasCheckConstraint(
                    "CK_Category_Slug",
                    "length(trim(slug)) > 0");

                t.HasCheckConstraint(
                    "CK_Category_NameEn",
                    "length(trim(name_en)) > 0");

                t.HasCheckConstraint(
                    "CK_Category_NameAr",
                    "length(trim(name_ar)) > 0");

                t.HasCheckConstraint(
                    "CK_Category_NotOwnParent",
                    "parent_category_id IS NULL OR parent_category_id <> category_id");
            });

            e.HasKey(x => x.CategoryId).HasName("PK_Category");

            e.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.ParentCategoryId)
                .HasColumnName("parent_category_id")
                .IsRequired(false);

            e.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.NameEn)
                .HasColumnName("name_en")
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.NameAr)
                .HasColumnName("name_ar")
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.DescriptionEn)
                .HasColumnName("description_en")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.DescriptionAr)
                .HasColumnName("description_ar")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.HasIndex(x => x.Slug)
                .HasDatabaseName("UQ_Category_Slug")
                .IsUnique();

            e.HasOne(x => x.ParentCategory)
                .WithMany(x => x.ChildCategories)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Product", t =>
            {
                t.HasCheckConstraint(
                    "CK_Product_Specifications",
                    "specifications IS NULL OR jsonb_typeof(specifications) = 'object'");

                t.HasCheckConstraint(
                    "CK_Product_Dates",
                    "updated_at >= created_at");

                t.HasCheckConstraint(
                    "CK_Product_NameEn",
                    "length(trim(name_en)) > 0");

                t.HasCheckConstraint(
                    "CK_Product_NameAr",
                    "length(trim(name_ar)) > 0");
            });

            e.HasKey(x => x.ProductId).HasName("PK_Product");

            e.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.NameEn)
                .HasColumnName("name_en")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.NameAr)
                .HasColumnName("name_ar")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.DescriptionEn)
                .HasColumnName("description_en")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.DescriptionAr)
                .HasColumnName("description_ar")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.Brand)
                .HasColumnName("brand")
                .HasMaxLength(150)
                .IsRequired(false);

            e.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.Specifications)
                .HasColumnName("specifications")
                .HasColumnType("jsonb")
                .IsRequired(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<ProductCategory>(e =>
        {
            e.ToTable("ProductCategory");

            e.HasKey(x => new { x.ProductId, x.CategoryId }).HasName("PK_ProductCategory");

            e.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            e.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            e.HasOne(x => x.Product)
                .WithMany(x => x.ProductCategories)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductCategory_Product");

            e.HasOne(x => x.Category)
                .WithMany(x => x.ProductCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductCategory_Category");
        });

        modelBuilder.Entity<ProductVariant>(e =>
        {
            e.ToTable("ProductVariant", t =>
            {
                t.HasCheckConstraint(
                    "CK_Variant_Code",
                    "length(trim(variant_code)) > 0");

                t.HasCheckConstraint(
                    "CK_Variant_Attributes",
                    "attributes IS NULL OR jsonb_typeof(attributes) = 'object'");
            });

            e.HasKey(x => x.VariantId).HasName("PK_ProductVariant");

            e.Property(x => x.VariantId)
                .HasColumnName("variant_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            e.Property(x => x.VariantCode)
                .HasColumnName("variant_code")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.Gtin)
                .HasColumnName("gtin")
                .HasMaxLength(14)
                .IsRequired(false);

            e.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.Attributes)
                .HasColumnName("attributes")
                .HasColumnType("jsonb")
                .IsRequired(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.HasIndex(x => x.Gtin)
                .HasDatabaseName("UQ_ProductVariant_Gtin")
                .IsUnique();

            e.HasIndex(x => new { x.ProductId, x.VariantCode })
                .HasDatabaseName("UQ_Variant_ProductCode")
                .IsUnique();

            e.HasOne(x => x.Product)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductVariant_Product");
        });

        modelBuilder.Entity<Merchant>(e =>
        {
            e.ToTable("Merchant", t =>
            {
                t.HasCheckConstraint(
                    "CK_Merchant_Name",
                    "length(trim(name)) > 0");
            });

            e.HasKey(x => x.MerchantId).HasName("PK_Merchant");

            e.Property(x => x.MerchantId)
                .HasColumnName("merchant_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.WebsiteUrl)
                .HasColumnName("website_url")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.SupportsAffiliateLinks)
                .HasColumnName("supports_affiliate_links")
                .IsRequired()
                .HasDefaultValue(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<Offer>(e =>
        {
            e.ToTable("Offer", t =>
            {
                t.HasCheckConstraint(
                    "CK_Offer_ProductUrl",
                    "length(trim(product_url)) > 0");

                t.HasCheckConstraint(
                    "CK_Offer_Price",
                    "price > 0");

                t.HasCheckConstraint(
                    "CK_Offer_Shipping",
                    "shipping_cost IS NULL OR shipping_cost >= 0");

                t.HasCheckConstraint(
                    "CK_Offer_Currency",
                    "currency ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_Offer_Attributes",
                    "attributes IS NULL OR jsonb_typeof(attributes) = 'object'");
            });

            e.HasKey(x => x.OfferId).HasName("PK_Offer");

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.MerchantId)
                .HasColumnName("merchant_id")
                .IsRequired();

            e.Property(x => x.VariantId)
                .HasColumnName("variant_id")
                .IsRequired();

            e.Property(x => x.ExternalOfferId)
                .HasColumnName("external_offer_id")
                .HasMaxLength(200)
                .IsRequired(false);

            e.Property(x => x.ProductUrl)
                .HasColumnName("product_url")
                .HasColumnType("text")
                .IsRequired();

            e.Property(x => x.AffiliateUrl)
                .HasColumnName("affiliate_url")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            e.Property(x => x.ShippingCost)
                .HasColumnName("shipping_cost")
                .HasPrecision(18, 2)
                .IsRequired(false);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            e.Property(x => x.IsAvailable)
                .HasColumnName("is_available")
                .IsRequired(false);

            e.Property(x => x.Attributes)
                .HasColumnName("attributes")
                .HasColumnType("jsonb")
                .IsRequired(false);

            e.Property(x => x.LastCheckedAt)
                .HasColumnName("last_checked_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            e.HasIndex(x => new { x.MerchantId, x.ExternalOfferId, x.VariantId })
                .HasDatabaseName("UX_Offer_Merchant_Listing_Variant")
                .IsUnique();

            e.HasIndex(x => new { x.VariantId, x.Currency, x.Price })
                .HasDatabaseName("IX_Offer_PriceComparison");

            e.HasOne(x => x.Merchant)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.MerchantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Offer_Merchant");

            e.HasOne(x => x.Variant)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.VariantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Offer_ProductVariant");
        });

        modelBuilder.Entity<PriceHistory>(e =>
        {
            e.ToTable("PriceHistory", t =>
            {
                t.HasCheckConstraint(
                    "CK_PriceHistory_Price",
                    "price > 0");

                t.HasCheckConstraint(
                    "CK_PriceHistory_Currency",
                    "currency ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.PriceHistoryId).HasName("PK_PriceHistory");

            e.Property(x => x.PriceHistoryId)
                .HasColumnName("price_history_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .IsRequired();

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            e.Property(x => x.RecordedAt)
                .HasColumnName("recorded_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasIndex(x => new { x.OfferId, x.RecordedAt })
                .HasDatabaseName("IX_PriceHistory_Offer_Date");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.PriceHistories)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceHistory_Offer");
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("User", t =>
            {
                t.HasCheckConstraint(
                    "CK_User_Name",
                    "length(trim(name)) > 0");

                t.HasCheckConstraint(
                    "CK_User_Email",
                    "length(trim(email)) > 0");
            });

            e.HasKey(x => x.UserId).HasName("PK_User");

            e.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("text")
                .IsRequired();

            e.Property(x => x.PreferredLanguageCode)
                .HasColumnName("preferred_language_code")
                .HasMaxLength(10)
                .IsRequired(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            // UX_User_Email is the unique expression index on LOWER(email).
            // Npgsql cannot model arbitrary expression indexes in Fluent API.
            // The initial migration creates it with raw SQL; preserve/recreate it
            // explicitly when authoring V2 migrations. Do not replace it with a
            // case-sensitive index or introduce a generated column here.

            e.HasOne(x => x.PreferredLanguage)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.PreferredLanguageCode)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_Language");
        });

        modelBuilder.Entity<SavedCollection>(e =>
        {
            e.ToTable("SavedCollection", t =>
            {
                t.HasCheckConstraint(
                    "CK_Collection_Dates",
                    "updated_at >= created_at");
            });

            e.HasKey(x => x.CollectionId).HasName("PK_SavedCollection");

            e.Property(x => x.CollectionId)
                .HasColumnName("collection_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired(false);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasOne(x => x.User)
                .WithMany(x => x.SavedCollections)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SavedCollection_User");
        });

        modelBuilder.Entity<SavedCollectionItem>(e =>
        {
            e.ToTable("SavedCollectionItem", t =>
            {
                t.HasCheckConstraint(
                    "CK_CollectionItem_Quantity",
                    "quantity > 0");
            });

            e.HasKey(x => x.CollectionItemId).HasName("PK_SavedCollectionItem");

            e.Property(x => x.CollectionItemId)
                .HasColumnName("collection_item_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.CollectionId)
                .HasColumnName("collection_id")
                .IsRequired();

            e.Property(x => x.VariantId)
                .HasColumnName("variant_id")
                .IsRequired();

            e.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .IsRequired()
                .HasDefaultValue(1);

            e.HasIndex(x => new { x.CollectionId, x.VariantId })
                .HasDatabaseName("UQ_Collection_Variant")
                .IsUnique();

            e.HasOne(x => x.Collection)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.CollectionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SavedCollectionItem_SavedCollection");

            e.HasOne(x => x.Variant)
                .WithMany(x => x.CollectionItems)
                .HasForeignKey(x => x.VariantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SavedCollectionItem_ProductVariant");
        });

        modelBuilder.Entity<ShareLink>(e =>
        {
            e.ToTable("ShareLink", t =>
            {
                t.HasCheckConstraint(
                    "CK_ShareLink_Slug",
                    "length(trim(slug)) > 0");

                t.HasCheckConstraint(
                    "CK_ShareLink_OneTarget",
                    "(product_id IS NOT NULL AND collection_id IS NULL) OR (product_id IS NULL AND collection_id IS NOT NULL)");
            });

            e.HasKey(x => x.ShareLinkId).HasName("PK_ShareLink");

            e.Property(x => x.ShareLinkId)
                .HasColumnName("share_link_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired(false);

            e.Property(x => x.CollectionId)
                .HasColumnName("collection_id")
                .IsRequired(false);

            e.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.HasIndex(x => x.Slug)
                .HasDatabaseName("UQ_ShareLink_Slug")
                .IsUnique();

            e.HasOne(x => x.Product)
                .WithMany(x => x.ShareLinks)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ShareLink_Product");

            e.HasOne(x => x.Collection)
                .WithMany(x => x.ShareLinks)
                .HasForeignKey(x => x.CollectionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ShareLink_SavedCollection");
        });

        modelBuilder.Entity<ShareLinkClick>(e =>
        {
            e.ToTable("ShareLinkClick");

            e.HasKey(x => x.ClickId).HasName("PK_ShareLinkClick");

            e.Property(x => x.ClickId)
                .HasColumnName("click_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.ShareLinkId)
                .HasColumnName("share_link_id")
                .IsRequired();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .IsRequired();

            e.Property(x => x.ClickedAt)
                .HasColumnName("clicked_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasOne(x => x.ShareLink)
                .WithMany(x => x.Clicks)
                .HasForeignKey(x => x.ShareLinkId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ShareLinkClick_ShareLink");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.ShareLinkClicks)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ShareLinkClick_Offer");
        });

        modelBuilder.Entity<AffiliateConversion>(e =>
        {
            e.ToTable("AffiliateConversion", t =>
            {
                t.HasCheckConstraint(
                    "CK_Conversion_StatusScope",
                    "status_scope = 'affiliate_conversion'");

                t.HasCheckConstraint(
                    "CK_Conversion_OrderAmount",
                    "order_amount IS NULL OR order_amount >= 0");

                t.HasCheckConstraint(
                    "CK_Conversion_Commission",
                    "commission_amount >= 0");

                t.HasCheckConstraint(
                    "CK_Conversion_Currency",
                    "currency ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.ConversionId).HasName("PK_AffiliateConversion");

            e.Property(x => x.ConversionId)
                .HasColumnName("conversion_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.MerchantId)
                .HasColumnName("merchant_id")
                .IsRequired();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .IsRequired(false);

            e.Property(x => x.ClickId)
                .HasColumnName("click_id")
                .IsRequired(false);

            e.Property(x => x.ExternalConversionId)
                .HasColumnName("external_conversion_id")
                .HasMaxLength(255)
                .IsRequired(false);

            e.Property(x => x.OrderAmount)
                .HasColumnName("order_amount")
                .HasPrecision(18, 2)
                .IsRequired(false);

            e.Property(x => x.CommissionAmount)
                .HasColumnName("commission_amount")
                .HasPrecision(18, 2)
                .IsRequired();

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            e.Property(x => x.StatusScope)
                .HasColumnName("status_scope")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("affiliate_conversion");

            e.Property(x => x.StatusCode)
                .HasColumnName("status_code")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("pending");

            e.Property(x => x.ReportedAt)
                .HasColumnName("reported_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasIndex(x => new { x.MerchantId, x.ExternalConversionId })
                .HasDatabaseName("UX_Conversion_Merchant_ExternalId")
                .IsUnique();

            e.HasOne(x => x.Merchant)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.MerchantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AffiliateConversion_Merchant");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AffiliateConversion_Offer");

            e.HasOne(x => x.Click)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.ClickId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AffiliateConversion_ShareLinkClick");

            e.HasOne(x => x.Status)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => new { x.StatusScope, x.StatusCode })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AffiliateConversion_Status");
        });

        modelBuilder.Entity<PriceAlert>(e =>
        {
            e.ToTable("PriceAlert", t =>
            {
                t.HasCheckConstraint(
                    "CK_Alert_TargetPrice",
                    "target_price > 0");

                t.HasCheckConstraint(
                    "CK_Alert_Currency",
                    "currency ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_Alert_LastTriggered",
                    "last_triggered_at IS NULL OR last_triggered_at >= created_at");
            });

            e.HasKey(x => x.AlertId).HasName("PK_PriceAlert");

            e.Property(x => x.AlertId)
                .HasColumnName("alert_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            e.Property(x => x.VariantId)
                .HasColumnName("variant_id")
                .IsRequired();

            e.Property(x => x.TargetPrice)
                .HasColumnName("target_price")
                .HasPrecision(18, 2)
                .IsRequired();

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.Property(x => x.LastTriggeredAt)
                .HasColumnName("last_triggered_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            e.HasIndex(x => new { x.VariantId, x.Currency, x.TargetPrice })
                .HasDatabaseName("IX_PriceAlert_Variant_Currency");

            e.HasOne(x => x.User)
                .WithMany(x => x.PriceAlerts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceAlert_User");

            e.HasOne(x => x.Variant)
                .WithMany(x => x.PriceAlerts)
                .HasForeignKey(x => x.VariantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceAlert_ProductVariant");
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notification");

            e.HasKey(x => x.NotificationId).HasName("PK_Notification");

            e.Property(x => x.NotificationId)
                .HasColumnName("notification_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            e.Property(x => x.AlertId)
                .HasColumnName("alert_id")
                .IsRequired(false);

            e.Property(x => x.Title)
                .HasColumnName("title")
                .HasColumnType("text")
                .IsRequired();

            e.Property(x => x.Message)
                .HasColumnName("message")
                .HasColumnType("text")
                .IsRequired();

            e.Property(x => x.IsRead)
                .HasColumnName("is_read")
                .IsRequired()
                .HasDefaultValue(false);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Notification_User");

            e.HasOne(x => x.Alert)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.AlertId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Notification_PriceAlert");
        });

        modelBuilder.Entity<CrawlRun>(e =>
        {
            e.ToTable("CrawlRun", t =>
            {
                t.HasCheckConstraint(
                    "CK_CrawlRun_StatusScope",
                    "status_scope = 'crawl_run'");

                t.HasCheckConstraint(
                    "CK_CrawlRun_ProductsFound",
                    "products_found >= 0");

                t.HasCheckConstraint(
                    "CK_CrawlRun_Dates",
                    "finished_at IS NULL OR (started_at IS NOT NULL AND finished_at >= started_at)");
            });

            e.HasKey(x => x.CrawlRunId).HasName("PK_CrawlRun");

            e.Property(x => x.CrawlRunId)
                .HasColumnName("crawl_run_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.MerchantId)
                .HasColumnName("merchant_id")
                .IsRequired();

            e.Property(x => x.StartedAt)
                .HasColumnName("started_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            e.Property(x => x.FinishedAt)
                .HasColumnName("finished_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            e.Property(x => x.StatusScope)
                .HasColumnName("status_scope")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("crawl_run");

            e.Property(x => x.StatusCode)
                .HasColumnName("status_code")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("pending");

            e.Property(x => x.ProductsFound)
                .HasColumnName("products_found")
                .IsRequired()
                .HasDefaultValue(0);

            e.Property(x => x.ErrorMessage)
                .HasColumnName("error_message")
                .HasColumnType("text")
                .IsRequired(false);

            e.HasOne(x => x.Merchant)
                .WithMany(x => x.CrawlRuns)
                .HasForeignKey(x => x.MerchantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawlRun_Merchant");

            e.HasOne(x => x.Status)
                .WithMany(x => x.CrawlRuns)
                .HasForeignKey(x => new { x.StatusScope, x.StatusCode })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawlRun_Status");
        });

        modelBuilder.Entity<CrawledListing>(e =>
        {
            e.ToTable("CrawledListing", t =>
            {
                t.HasCheckConstraint(
                    "CK_Listing_StatusScope",
                    "status_scope = 'listing_match'");

                t.HasCheckConstraint(
                    "CK_Listing_Price",
                    "price IS NULL OR price > 0");

                t.HasCheckConstraint(
                    "CK_Listing_PriceCurrency",
                    "price IS NULL OR currency IS NOT NULL");

                t.HasCheckConstraint(
                    "CK_Listing_Currency",
                    "currency IS NULL OR currency ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_Listing_Specifications",
                    "raw_specifications IS NULL OR jsonb_typeof(raw_specifications) = 'object'");
            });

            e.HasKey(x => x.ListingId).HasName("PK_CrawledListing");

            e.Property(x => x.ListingId)
                .HasColumnName("listing_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.CrawlRunId)
                .HasColumnName("crawl_run_id")
                .IsRequired();

            e.Property(x => x.MatchedVariantId)
                .HasColumnName("matched_variant_id")
                .IsRequired(false);

            e.Property(x => x.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.ExternalOfferId)
                .HasColumnName("external_offer_id")
                .HasMaxLength(200)
                .IsRequired(false);

            e.Property(x => x.ProductUrl)
                .HasColumnName("product_url")
                .HasColumnType("text")
                .IsRequired();

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired(false);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired(false);

            e.Property(x => x.RawSpecifications)
                .HasColumnName("raw_specifications")
                .HasColumnType("jsonb")
                .IsRequired(false);

            e.Property(x => x.StatusScope)
                .HasColumnName("status_scope")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("listing_match");

            e.Property(x => x.StatusCode)
                .HasColumnName("status_code")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("pending");

            e.Property(x => x.CrawledAt)
                .HasColumnName("crawled_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("now()");

            e.HasOne(x => x.CrawlRun)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.CrawlRunId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawledListing_CrawlRun");

            e.HasOne(x => x.MatchedVariant)
                .WithMany(x => x.CrawledListings)
                .HasForeignKey(x => x.MatchedVariantId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CrawledListing_ProductVariant");

            e.HasOne(x => x.Status)
                .WithMany(x => x.CrawledListings)
                .HasForeignKey(x => new { x.StatusScope, x.StatusCode })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawledListing_Status");
        });

        modelBuilder.Entity<SponsoredListing>(e =>
        {
            e.ToTable("SponsoredListing", t =>
            {
                t.HasCheckConstraint(
                    "CK_Sponsored_Dates",
                    "end_date > start_date");

                t.HasCheckConstraint(
                    "CK_Sponsored_Fee",
                    "sponsorship_fee IS NULL OR sponsorship_fee >= 0");

                t.HasCheckConstraint(
                    "CK_Sponsored_Currency",
                    "currency ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.SponsoredListingId).HasName("PK_SponsoredListing");

            e.Property(x => x.SponsoredListingId)
                .HasColumnName("sponsored_listing_id")
                .IsRequired()
                .UseIdentityByDefaultColumn();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .IsRequired();

            e.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            e.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            e.Property(x => x.SponsorshipFee)
                .HasColumnName("sponsorship_fee")
                .HasPrecision(18, 2)
                .IsRequired(false);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.HasOne(x => x.Offer)
                .WithMany(x => x.SponsoredListings)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SponsoredListing_Offer");
        });

        modelBuilder.Entity<Status>(e =>
        {
            e.ToTable("Status", t =>
            {
                t.HasCheckConstraint(
                    "CK_Status_LabelEn",
                    "length(trim(label_en)) > 0");

                t.HasCheckConstraint(
                    "CK_Status_LabelAr",
                    "length(trim(label_ar)) > 0");
            });

            e.HasKey(x => new { x.Scope, x.Code }).HasName("PK_Status");

            e.Property(x => x.Scope)
                .HasColumnName("scope")
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.LabelEn)
                .HasColumnName("label_en")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.LabelAr)
                .HasColumnName("label_ar")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            e.Property(x => x.SortOrder)
                .HasColumnName("sort_order")
                .IsRequired()
                .HasDefaultValue(0);

            e.HasData(
                new { Scope = "crawl_run", Code = "pending", LabelEn = "Pending", LabelAr = "قيد الانتظار", IsActive = true, SortOrder = 0 },
                new { Scope = "crawl_run", Code = "running", LabelEn = "Running", LabelAr = "قيد التنفيذ", IsActive = true, SortOrder = 10 },
                new { Scope = "crawl_run", Code = "completed", LabelEn = "Completed", LabelAr = "مكتمل", IsActive = true, SortOrder = 20 },
                new { Scope = "crawl_run", Code = "failed", LabelEn = "Failed", LabelAr = "فشل", IsActive = true, SortOrder = 30 },

                new { Scope = "listing_match", Code = "pending", LabelEn = "Pending", LabelAr = "قيد الانتظار", IsActive = true, SortOrder = 0 },
                new { Scope = "listing_match", Code = "matched", LabelEn = "Matched", LabelAr = "مطابق", IsActive = true, SortOrder = 10 },
                new { Scope = "listing_match", Code = "unmatched", LabelEn = "Unmatched", LabelAr = "غير مطابق", IsActive = true, SortOrder = 20 },
                new { Scope = "listing_match", Code = "review", LabelEn = "Needs review", LabelAr = "يحتاج مراجعة", IsActive = true, SortOrder = 30 },

                new { Scope = "affiliate_conversion", Code = "pending", LabelEn = "Pending", LabelAr = "قيد الانتظار", IsActive = true, SortOrder = 0 },
                new { Scope = "affiliate_conversion", Code = "approved", LabelEn = "Approved", LabelAr = "تمت الموافقة", IsActive = true, SortOrder = 10 },
                new { Scope = "affiliate_conversion", Code = "rejected", LabelEn = "Rejected", LabelAr = "مرفوض", IsActive = true, SortOrder = 20 },
                new { Scope = "affiliate_conversion", Code = "paid", LabelEn = "Paid", LabelAr = "مدفوع", IsActive = true, SortOrder = 30 });
        });
    }
}
