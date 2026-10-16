
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<Retailer> Retailers => Set<Retailer>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();
    public DbSet<User> Users => Set<User>();
    public DbSet<PCBuild> PCBuilds => Set<PCBuild>();
    public DbSet<PCBuildItem> PCBuildItems => Set<PCBuildItem>();
    public DbSet<MagicLink> MagicLinks => Set<MagicLink>();
    public DbSet<MagicLinkClick> MagicLinkClicks => Set<MagicLinkClick>();
    public DbSet<PriceAlert> PriceAlerts => Set<PriceAlert>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CrawlRun> CrawlRuns => Set<CrawlRun>();
    public DbSet<CrawledListing> CrawledListings => Set<CrawledListing>();
    public DbSet<AffiliateConversion> AffiliateConversions => Set<AffiliateConversion>();
    public DbSet<SponsoredListing> SponsoredListings => Set<SponsoredListing>();

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove(
            typeof(ForeignKeyIndexConvention));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Category", t =>
            {
                t.HasCheckConstraint(
                    "CK_Category_Name",
                    "LENGTH(TRIM(\"name\")) > 0");
            });

            e.HasKey(x => x.CategoryId).HasName("PK_Category");

            e.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(50);

            e.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(255);

            e.HasAlternateKey(x => x.Name)
                .HasName("UQ_Category_Name");
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.ToTable("Component", t =>
            {
                t.HasCheckConstraint(
                    "CK_Component_Brand",
                    "LENGTH(TRIM(\"brand\")) > 0");

                t.HasCheckConstraint(
                    "CK_Component_Model",
                    "LENGTH(TRIM(\"model\")) > 0");

                t.HasCheckConstraint(
                    "CK_Component_Specifications",
                    "\"specifications\" IS NULL OR jsonb_typeof(\"specifications\") = 'object'");
            });

            e.HasKey(x => x.ComponentId).HasName("PK_Component");

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.CategoryId)
                .HasColumnName("category_id");

            e.Property(x => x.Brand)
                .HasColumnName("brand")
                .HasMaxLength(255);

            e.Property(x => x.Model)
                .HasColumnName("model")
                .HasMaxLength(255);

            e.Property(x => x.ManufacturerPartNumber)
                .HasColumnName("manufacturer_part_number")
                .HasMaxLength(255);

            e.Property(x => x.Specifications)
                .HasColumnName("specifications")
                .HasColumnType("jsonb");

            e.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasColumnType("text");

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            e.HasOne(x => x.Category)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Component_Category");

            e.HasIndex(x => new
            {
                x.Brand,
                x.ManufacturerPartNumber
            })
            .HasDatabaseName("UX_Component_Brand_MPN")
            .IsUnique()
            .HasFilter("\"manufacturer_part_number\" IS NOT NULL");
        });

        modelBuilder.Entity<Retailer>(e =>
        {
            e.ToTable("Retailer", t =>
            {
                t.HasCheckConstraint(
                    "CK_Retailer_Name",
                    "LENGTH(TRIM(\"name\")) > 0");
            });

            e.HasKey(x => x.RetailerId).HasName("PK_Retailer");

            e.Property(x => x.RetailerId)
                .HasColumnName("retailer_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            e.Property(x => x.WebsiteUrl)
                .HasColumnName("website_url")
                .HasColumnType("text");

            e.Property(x => x.SupportsAffiliateLinks)
                .HasColumnName("supports_affiliate_links")
                .HasDefaultValue(false);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            e.HasAlternateKey(x => x.WebsiteUrl)
                .HasName("UQ_Retailer_Website");
        });

        modelBuilder.Entity<Offer>(e =>
        {
            e.ToTable("Offer", t =>
            {
                t.HasCheckConstraint(
                    "CK_Offer_Price",
                    "\"price\" > 0");

                t.HasCheckConstraint(
                    "CK_Offer_ShippingCost",
                    "\"shipping_cost\" >= 0");

                t.HasCheckConstraint(
                    "CK_Offer_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.OfferId).HasName("PK_Offer");

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.RetailerId)
                .HasColumnName("retailer_id");

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id");

            e.Property(x => x.ExternalProductId)
                .HasColumnName("external_product_id")
                .HasMaxLength(150);

            e.Property(x => x.ProductUrl)
                .HasColumnName("product_url")
                .HasColumnType("text");

            e.Property(x => x.AffiliateUrl)
                .HasColumnName("affiliate_url")
                .HasColumnType("text");

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2);

            e.Property(x => x.ShippingCost)
                .HasColumnName("shipping_cost")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.IsAvailable)
                .HasColumnName("is_available");

            e.Property(x => x.LastCheckedAt)
                .HasColumnName("last_checked_at");

            e.HasAlternateKey(x => new
            {
                x.RetailerId,
                x.ProductUrl
            })
            .HasName("UQ_Offer_Retailer_ProductUrl");

            e.HasOne(x => x.Retailer)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.RetailerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Offer_Retailer");

            e.HasOne(x => x.Component)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Offer_Component");

            e.HasIndex(x => new
            {
                x.ComponentId,
                x.Currency,
                x.Price
            })
            .HasDatabaseName("IX_Offer_PriceComparison")
            .HasFilter("\"is_available\" IS TRUE");

            e.HasIndex(x => new
            {
                x.RetailerId,
                x.ExternalProductId
            })
            .HasDatabaseName("UX_Offer_Retailer_ExternalProduct")
            .IsUnique()
            .HasFilter("\"external_product_id\" IS NOT NULL");
        });

        modelBuilder.Entity<PriceHistory>(e =>
        {
            e.ToTable("PriceHistory", t =>
            {
                t.HasCheckConstraint(
                    "CK_PriceHistory_Price",
                    "\"price\" > 0");

                t.HasCheckConstraint(
                    "CK_PriceHistory_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.PriceHistoryId)
                .HasName("PK_PriceHistory");

            e.Property(x => x.PriceHistoryId)
                .HasColumnName("price_history_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id");

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.RecordedAt)
                .HasColumnName("recorded_at")
                .HasDefaultValueSql("NOW()");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.PriceHistories)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceHistory_Offer");

            e.HasIndex(x => new
            {
                x.OfferId,
                x.RecordedAt
            })
            .HasDatabaseName("IX_PriceHistory_Offer_RecordedAt")
            .IsDescending(false, true);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("User", t =>
            {
                t.HasCheckConstraint(
                    "CK_User_Name",
                    "LENGTH(TRIM(\"name\")) > 0");

                t.HasCheckConstraint(
                    "CK_User_Email",
                    "LENGTH(TRIM(\"email\")) > 0");
            });

            e.HasKey(x => x.UserId).HasName("PK_User");

            e.Property(x => x.UserId)
                .HasColumnName("user_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            e.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255);

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("text");

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<PCBuild>(e =>
        {
            e.ToTable("PCBuild", t =>
            {
                t.HasCheckConstraint(
                    "CK_PCBuild_Budget",
                    "\"budget\" >= 0");

                t.HasCheckConstraint(
                    "CK_PCBuild_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_PCBuild_Name",
                    "LENGTH(TRIM(\"name\")) > 0");

                t.HasCheckConstraint(
                    "CK_PCBuild_Dates",
                    "\"updated_at\" >= \"created_at\"");
            });

            e.HasKey(x => x.BuildId).HasName("PK_PCBuild");

            e.Property(x => x.BuildId)
                .HasColumnName("build_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            e.Property(x => x.Budget)
                .HasColumnName("budget")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            e.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");

            e.HasOne(x => x.User)
                .WithMany(x => x.PCBuilds)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PCBuild_User");
        });

        modelBuilder.Entity<PCBuildItem>(e =>
        {
            e.ToTable("PCBuildItem", t =>
            {
                t.HasCheckConstraint(
                    "CK_PCBuildItem_Quantity",
                    "\"quantity\" > 0");
            });

            e.HasKey(x => x.BuildItemId)
                .HasName("PK_PCBuildItem");

            e.Property(x => x.BuildItemId)
                .HasColumnName("build_item_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.BuildId)
                .HasColumnName("build_id");

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id");

            e.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasDefaultValue(1);

            e.HasAlternateKey(x => new
            {
                x.BuildId,
                x.ComponentId
            })
            .HasName("UQ_PCBuildItem_Build_Component");

            e.HasOne(x => x.Build)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.BuildId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PCBuildItem_PCBuild");

            e.HasOne(x => x.Component)
                .WithMany(x => x.BuildItems)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PCBuildItem_Component");
        });

        modelBuilder.Entity<MagicLink>(e =>
        {
            e.ToTable("MagicLink", t =>
            {
                t.HasCheckConstraint(
                    "CK_MagicLink_Slug",
                    "LENGTH(TRIM(\"slug\")) > 0");

                t.HasCheckConstraint(
                    "CK_MagicLink_ExactlyOneTarget",
                    "(\"component_id\" IS NOT NULL AND \"build_id\" IS NULL) OR (\"component_id\" IS NULL AND \"build_id\" IS NOT NULL)");
            });

            e.HasKey(x => x.MagicLinkId)
                .HasName("PK_MagicLink");

            e.Property(x => x.MagicLinkId)
                .HasColumnName("magic_link_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id");

            e.Property(x => x.BuildId)
                .HasColumnName("build_id");

            e.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(255);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            e.HasAlternateKey(x => x.Slug)
                .HasName("UQ_MagicLink_Slug");

            e.HasOne(x => x.Component)
                .WithMany(x => x.MagicLinks)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MagicLink_Component");

            e.HasOne(x => x.Build)
                .WithMany(x => x.MagicLinks)
                .HasForeignKey(x => x.BuildId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MagicLink_PCBuild");
        });

        modelBuilder.Entity<MagicLinkClick>(e =>
        {
            e.ToTable("MagicLinkClick");

            e.HasKey(x => x.ClickId)
                .HasName("PK_MagicLinkClick");

            e.Property(x => x.ClickId)
                .HasColumnName("click_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.MagicLinkId)
                .HasColumnName("magic_link_id");

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id");

            e.Property(x => x.ClickedAt)
                .HasColumnName("clicked_at")
                .HasDefaultValueSql("NOW()");

            e.HasOne(x => x.MagicLink)
                .WithMany(x => x.Clicks)
                .HasForeignKey(x => x.MagicLinkId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MagicLinkClick_MagicLink");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.MagicLinkClicks)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_MagicLinkClick_Offer");
        });

        modelBuilder.Entity<PriceAlert>(e =>
        {
            e.ToTable("PriceAlert", t =>
            {
                t.HasCheckConstraint(
                    "CK_PriceAlert_TargetPrice",
                    "\"target_price\" > 0");

                t.HasCheckConstraint(
                    "CK_PriceAlert_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_PriceAlert_LastTriggered",
                    "\"last_triggered_at\" IS NULL OR \"last_triggered_at\" >= \"created_at\"");
            });

            e.HasKey(x => x.AlertId)
                .HasName("PK_PriceAlert");

            e.Property(x => x.AlertId)
                .HasColumnName("alert_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id");

            e.Property(x => x.TargetPrice)
                .HasColumnName("target_price")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            e.Property(x => x.LastTriggeredAt)
                .HasColumnName("last_triggered_at");

            e.HasOne(x => x.User)
                .WithMany(x => x.PriceAlerts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceAlert_User");

            e.HasOne(x => x.Component)
                .WithMany(x => x.PriceAlerts)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PriceAlert_Component");

            e.HasIndex(x => new
            {
                x.ComponentId,
                x.Currency,
                x.TargetPrice
            })
            .HasDatabaseName("IX_PriceAlert_Active_Component")
            .HasFilter("\"is_active\" IS TRUE");
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notification", t =>
            {
                t.HasCheckConstraint(
                    "CK_Notification_Title",
                    "LENGTH(TRIM(\"title\")) > 0");

                t.HasCheckConstraint(
                    "CK_Notification_Message",
                    "LENGTH(TRIM(\"message\")) > 0");
            });

            e.HasKey(x => x.NotificationId)
                .HasName("PK_Notification");

            e.Property(x => x.NotificationId)
                .HasColumnName("notification_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.AlertId)
                .HasColumnName("alert_id");

            e.Property(x => x.Title)
                .HasColumnName("title")
                .HasColumnType("text");

            e.Property(x => x.Message)
                .HasColumnName("message")
                .HasColumnType("text");

            e.Property(x => x.IsRead)
                .HasColumnName("is_read")
                .HasDefaultValue(false);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

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
                    "CK_CrawlRun_ProductsFound",
                    "\"products_found\" >= 0");

                t.HasCheckConstraint(
                    "CK_CrawlRun_Status",
                    "\"status\" IN ('Pending','Running','Completed','Failed')");

                t.HasCheckConstraint(
                    "CK_CrawlRun_Dates",
                    "\"finished_at\" IS NULL OR (\"started_at\" IS NOT NULL AND \"finished_at\" >= \"started_at\")");

                t.HasCheckConstraint(
                    "CK_CrawlRun_Completion",
                    "(\"status\" = 'Pending' AND \"started_at\" IS NULL AND \"finished_at\" IS NULL) OR (\"status\" = 'Running' AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NULL) OR (\"status\" IN ('Completed','Failed') AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NOT NULL)");
            });

            e.HasKey(x => x.CrawlRunId)
                .HasName("PK_CrawlRun");

            e.Property(x => x.CrawlRunId)
                .HasColumnName("crawl_run_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.RetailerId)
                .HasColumnName("retailer_id");

            e.Property(x => x.StartedAt)
                .HasColumnName("started_at");

            e.Property(x => x.FinishedAt)
                .HasColumnName("finished_at");

            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(30)
                .HasDefaultValue("Pending");

            e.Property(x => x.ProductsFound)
                .HasColumnName("products_found")
                .HasDefaultValue(0);

            e.Property(x => x.ErrorMessage)
                .HasColumnName("error_message")
                .HasColumnType("text");

            e.HasOne(x => x.Retailer)
                .WithMany(x => x.CrawlRuns)
                .HasForeignKey(x => x.RetailerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawlRun_Retailer");
        });

        modelBuilder.Entity<CrawledListing>(e =>
        {
            e.ToTable("CrawledListing", t =>
            {
                t.HasCheckConstraint(
                    "CK_CrawledListing_Price",
                    "\"price\" > 0");

                t.HasCheckConstraint(
                    "CK_CrawledListing_Currency",
                    "\"currency\" IS NULL OR \"currency\" ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_CrawledListing_PriceCurrency",
                    "\"price\" IS NULL OR \"currency\" IS NOT NULL");

                t.HasCheckConstraint(
                    "CK_CrawledListing_Status",
                    "\"match_status\" IN ('Pending','Matched','Unmatched','Review')");

                t.HasCheckConstraint(
                    "CK_CrawledListing_MatchedComponent",
                    "\"match_status\" <> 'Matched' OR \"component_id\" IS NOT NULL");

                t.HasCheckConstraint(
                    "CK_CrawledListing_Specifications",
                    "\"raw_specifications\" IS NULL OR jsonb_typeof(\"raw_specifications\") = 'object'");
            });

            e.HasKey(x => x.ListingId)
                .HasName("PK_CrawledListing");

            e.Property(x => x.ListingId)
                .HasColumnName("listing_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.CrawlRunId)
                .HasColumnName("crawl_run_id");

            e.Property(x => x.ComponentId)
                .HasColumnName("component_id");

            e.Property(x => x.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(255);

            e.Property(x => x.ExternalProductId)
                .HasColumnName("external_product_id")
                .HasMaxLength(255);

            e.Property(x => x.ProductUrl)
                .HasColumnName("product_url")
                .HasColumnType("text");

            e.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.RawSpecifications)
                .HasColumnName("raw_specifications")
                .HasColumnType("jsonb");

            e.Property(x => x.MatchStatus)
                .HasColumnName("match_status")
                .HasMaxLength(30)
                .HasDefaultValue("Pending");

            e.Property(x => x.CrawledAt)
                .HasColumnName("crawled_at")
                .HasDefaultValueSql("NOW()");

            e.HasOne(x => x.CrawlRun)
                .WithMany(x => x.Listings)
                .HasForeignKey(x => x.CrawlRunId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawledListing_CrawlRun");

            e.HasOne(x => x.Component)
                .WithMany(x => x.CrawledListings)
                .HasForeignKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CrawledListing_Component");
        });

        modelBuilder.Entity<AffiliateConversion>(e =>
        {
            e.ToTable("AffiliateConversion", t =>
            {
                t.HasCheckConstraint(
                    "CK_AffiliateConversion_OrderAmount",
                    "\"order_amount\" >= 0");

                t.HasCheckConstraint(
                    "CK_AffiliateConversion_Commission",
                    "\"commission_amount\" >= 0");

                t.HasCheckConstraint(
                    "CK_AffiliateConversion_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");

                t.HasCheckConstraint(
                    "CK_AffiliateConversion_Status",
                    "\"status\" IN ('Pending','Approved','Rejected','Paid')");
            });

            e.HasKey(x => x.ConversionId)
                .HasName("PK_AffiliateConversion");

            e.Property(x => x.ConversionId)
                .HasColumnName("conversion_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.RetailerId)
                .HasColumnName("retailer_id");

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id");

            e.Property(x => x.ClickId)
                .HasColumnName("click_id");

            e.Property(x => x.ExternalConversionId)
                .HasColumnName("external_conversion_id")
                .HasMaxLength(255);

            e.Property(x => x.OrderAmount)
                .HasColumnName("order_amount")
                .HasPrecision(18, 2);

            e.Property(x => x.CommissionAmount)
                .HasColumnName("commission_amount")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(30)
                .HasDefaultValue("Pending");

            e.Property(x => x.ReportedAt)
                .HasColumnName("reported_at");

            e.HasOne(x => x.Retailer)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.RetailerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AffiliateConversion_Retailer");

            e.HasOne(x => x.Offer)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AffiliateConversion_Offer");

            e.HasOne(x => x.Click)
                .WithMany(x => x.AffiliateConversions)
                .HasForeignKey(x => x.ClickId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AffiliateConversion_Click");

            e.HasIndex(x => new
            {
                x.RetailerId,
                x.ExternalConversionId
            })
            .HasDatabaseName("UX_AffiliateConversion_External")
            .IsUnique()
            .HasFilter("\"external_conversion_id\" IS NOT NULL");
        });

        modelBuilder.Entity<SponsoredListing>(e =>
        {
            e.ToTable("SponsoredListing", t =>
            {
                t.HasCheckConstraint(
                    "CK_SponsoredListing_Dates",
                    "\"end_date\" > \"start_date\"");

                t.HasCheckConstraint(
                    "CK_SponsoredListing_Fee",
                    "\"sponsorship_fee\" >= 0");

                t.HasCheckConstraint(
                    "CK_SponsoredListing_Currency",
                    "\"currency\" ~ '^[A-Z]{3}$'");
            });

            e.HasKey(x => x.SponsoredListingId)
                .HasName("PK_SponsoredListing");

            e.Property(x => x.SponsoredListingId)
                .HasColumnName("sponsored_listing_id")
                .UseIdentityByDefaultColumn();

            e.Property(x => x.OfferId)
                .HasColumnName("offer_id");

            e.Property(x => x.StartDate)
                .HasColumnName("start_date");

            e.Property(x => x.EndDate)
                .HasColumnName("end_date");

            e.Property(x => x.SponsorshipFee)
                .HasColumnName("sponsorship_fee")
                .HasPrecision(18, 2);

            e.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3);

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            e.HasOne(x => x.Offer)
                .WithMany(x => x.SponsoredListings)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SponsoredListing_Offer");
        });
    }
}
