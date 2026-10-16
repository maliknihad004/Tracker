using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.category_id);
                    table.UniqueConstraint("UQ_Category_Name", x => x.name);
                    table.CheckConstraint("CK_Category_Name", "LENGTH(TRIM(\"name\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Retailer",
                columns: table => new
                {
                    retailer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    website_url = table.Column<string>(type: "text", nullable: false),
                    supports_affiliate_links = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailer", x => x.retailer_id);
                    table.UniqueConstraint("UQ_Retailer_Website", x => x.website_url);
                    table.CheckConstraint("CK_Retailer_Name", "LENGTH(TRIM(\"name\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.CheckConstraint("CK_User_Email", "LENGTH(TRIM(\"email\")) > 0");
                    table.CheckConstraint("CK_User_Name", "LENGTH(TRIM(\"name\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    component_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    brand = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    manufacturer_part_number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    specifications = table.Column<string>(type: "jsonb", nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.component_id);
                    table.CheckConstraint("CK_Component_Brand", "LENGTH(TRIM(\"brand\")) > 0");
                    table.CheckConstraint("CK_Component_Model", "LENGTH(TRIM(\"model\")) > 0");
                    table.CheckConstraint("CK_Component_Specifications", "\"specifications\" IS NULL OR jsonb_typeof(\"specifications\") = 'object'");
                    table.ForeignKey(
                        name: "FK_Component_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrawlRun",
                columns: table => new
                {
                    crawl_run_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    retailer_id = table.Column<int>(type: "integer", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "Pending"),
                    products_found = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    error_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlRun", x => x.crawl_run_id);
                    table.CheckConstraint("CK_CrawlRun_Completion", "(\"status\" = 'Pending' AND \"started_at\" IS NULL AND \"finished_at\" IS NULL) OR (\"status\" = 'Running' AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NULL) OR (\"status\" IN ('Completed','Failed') AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NOT NULL)");
                    table.CheckConstraint("CK_CrawlRun_Dates", "\"finished_at\" IS NULL OR (\"started_at\" IS NOT NULL AND \"finished_at\" >= \"started_at\")");
                    table.CheckConstraint("CK_CrawlRun_ProductsFound", "\"products_found\" >= 0");
                    table.CheckConstraint("CK_CrawlRun_Status", "\"status\" IN ('Pending','Running','Completed','Failed')");
                    table.ForeignKey(
                        name: "FK_CrawlRun_Retailer",
                        column: x => x.retailer_id,
                        principalTable: "Retailer",
                        principalColumn: "retailer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PCBuild",
                columns: table => new
                {
                    build_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    budget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBuild", x => x.build_id);
                    table.CheckConstraint("CK_PCBuild_Budget", "\"budget\" >= 0");
                    table.CheckConstraint("CK_PCBuild_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_PCBuild_Dates", "\"updated_at\" >= \"created_at\"");
                    table.CheckConstraint("CK_PCBuild_Name", "LENGTH(TRIM(\"name\")) > 0");
                    table.ForeignKey(
                        name: "FK_PCBuild_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    offer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    retailer_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<int>(type: "integer", nullable: false),
                    external_product_id = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    product_url = table.Column<string>(type: "text", nullable: false),
                    affiliate_url = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    shipping_cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: true),
                    last_checked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.offer_id);
                    table.UniqueConstraint("UQ_Offer_Retailer_ProductUrl", x => new { x.retailer_id, x.product_url });
                    table.CheckConstraint("CK_Offer_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Offer_Price", "\"price\" > 0");
                    table.CheckConstraint("CK_Offer_ShippingCost", "\"shipping_cost\" >= 0");
                    table.ForeignKey(
                        name: "FK_Offer_Component",
                        column: x => x.component_id,
                        principalTable: "Component",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offer_Retailer",
                        column: x => x.retailer_id,
                        principalTable: "Retailer",
                        principalColumn: "retailer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceAlert",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<int>(type: "integer", nullable: false),
                    target_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    last_triggered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAlert", x => x.alert_id);
                    table.CheckConstraint("CK_PriceAlert_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_PriceAlert_LastTriggered", "\"last_triggered_at\" IS NULL OR \"last_triggered_at\" >= \"created_at\"");
                    table.CheckConstraint("CK_PriceAlert_TargetPrice", "\"target_price\" > 0");
                    table.ForeignKey(
                        name: "FK_PriceAlert_Component",
                        column: x => x.component_id,
                        principalTable: "Component",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceAlert_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrawledListing",
                columns: table => new
                {
                    listing_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crawl_run_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<int>(type: "integer", nullable: true),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    external_product_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    product_url = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    raw_specifications = table.Column<string>(type: "jsonb", nullable: true),
                    match_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "Pending"),
                    crawled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawledListing", x => x.listing_id);
                    table.CheckConstraint("CK_CrawledListing_Currency", "\"currency\" IS NULL OR \"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_CrawledListing_MatchedComponent", "\"match_status\" <> 'Matched' OR \"component_id\" IS NOT NULL");
                    table.CheckConstraint("CK_CrawledListing_Price", "\"price\" > 0");
                    table.CheckConstraint("CK_CrawledListing_PriceCurrency", "\"price\" IS NULL OR \"currency\" IS NOT NULL");
                    table.CheckConstraint("CK_CrawledListing_Specifications", "\"raw_specifications\" IS NULL OR jsonb_typeof(\"raw_specifications\") = 'object'");
                    table.CheckConstraint("CK_CrawledListing_Status", "\"match_status\" IN ('Pending','Matched','Unmatched','Review')");
                    table.ForeignKey(
                        name: "FK_CrawledListing_Component",
                        column: x => x.component_id,
                        principalTable: "Component",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrawledListing_CrawlRun",
                        column: x => x.crawl_run_id,
                        principalTable: "CrawlRun",
                        principalColumn: "crawl_run_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MagicLink",
                columns: table => new
                {
                    magic_link_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<int>(type: "integer", nullable: true),
                    build_id = table.Column<int>(type: "integer", nullable: true),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicLink", x => x.magic_link_id);
                    table.UniqueConstraint("UQ_MagicLink_Slug", x => x.slug);
                    table.CheckConstraint("CK_MagicLink_ExactlyOneTarget", "(\"component_id\" IS NOT NULL AND \"build_id\" IS NULL) OR (\"component_id\" IS NULL AND \"build_id\" IS NOT NULL)");
                    table.CheckConstraint("CK_MagicLink_Slug", "LENGTH(TRIM(\"slug\")) > 0");
                    table.ForeignKey(
                        name: "FK_MagicLink_Component",
                        column: x => x.component_id,
                        principalTable: "Component",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MagicLink_PCBuild",
                        column: x => x.build_id,
                        principalTable: "PCBuild",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PCBuildItem",
                columns: table => new
                {
                    build_item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    build_id = table.Column<int>(type: "integer", nullable: false),
                    component_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBuildItem", x => x.build_item_id);
                    table.UniqueConstraint("UQ_PCBuildItem_Build_Component", x => new { x.build_id, x.component_id });
                    table.CheckConstraint("CK_PCBuildItem_Quantity", "\"quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_PCBuildItem_Component",
                        column: x => x.component_id,
                        principalTable: "Component",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PCBuildItem_PCBuild",
                        column: x => x.build_id,
                        principalTable: "PCBuild",
                        principalColumn: "build_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    price_history_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    offer_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    recorded_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => x.price_history_id);
                    table.CheckConstraint("CK_PriceHistory_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_PriceHistory_Price", "\"price\" > 0");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SponsoredListing",
                columns: table => new
                {
                    sponsored_listing_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    offer_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    sponsorship_fee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsoredListing", x => x.sponsored_listing_id);
                    table.CheckConstraint("CK_SponsoredListing_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_SponsoredListing_Dates", "\"end_date\" > \"start_date\"");
                    table.CheckConstraint("CK_SponsoredListing_Fee", "\"sponsorship_fee\" >= 0");
                    table.ForeignKey(
                        name: "FK_SponsoredListing_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    alert_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.notification_id);
                    table.CheckConstraint("CK_Notification_Message", "LENGTH(TRIM(\"message\")) > 0");
                    table.CheckConstraint("CK_Notification_Title", "LENGTH(TRIM(\"title\")) > 0");
                    table.ForeignKey(
                        name: "FK_Notification_PriceAlert",
                        column: x => x.alert_id,
                        principalTable: "PriceAlert",
                        principalColumn: "alert_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MagicLinkClick",
                columns: table => new
                {
                    click_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    magic_link_id = table.Column<int>(type: "integer", nullable: false),
                    offer_id = table.Column<int>(type: "integer", nullable: false),
                    clicked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicLinkClick", x => x.click_id);
                    table.ForeignKey(
                        name: "FK_MagicLinkClick_MagicLink",
                        column: x => x.magic_link_id,
                        principalTable: "MagicLink",
                        principalColumn: "magic_link_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MagicLinkClick_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AffiliateConversion",
                columns: table => new
                {
                    conversion_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    retailer_id = table.Column<int>(type: "integer", nullable: false),
                    offer_id = table.Column<int>(type: "integer", nullable: true),
                    click_id = table.Column<int>(type: "integer", nullable: true),
                    external_conversion_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    order_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    commission_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "Pending"),
                    reported_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliateConversion", x => x.conversion_id);
                    table.CheckConstraint("CK_AffiliateConversion_Commission", "\"commission_amount\" >= 0");
                    table.CheckConstraint("CK_AffiliateConversion_Currency", "\"currency\" ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_AffiliateConversion_OrderAmount", "\"order_amount\" >= 0");
                    table.CheckConstraint("CK_AffiliateConversion_Status", "\"status\" IN ('Pending','Approved','Rejected','Paid')");
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Click",
                        column: x => x.click_id,
                        principalTable: "MagicLinkClick",
                        principalColumn: "click_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Retailer",
                        column: x => x.retailer_id,
                        principalTable: "Retailer",
                        principalColumn: "retailer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UX_AffiliateConversion_External",
                table: "AffiliateConversion",
                columns: new[] { "retailer_id", "external_conversion_id" },
                unique: true,
                filter: "\"external_conversion_id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_Component_Brand_MPN",
                table: "Component",
                columns: new[] { "brand", "manufacturer_part_number" },
                unique: true,
                filter: "\"manufacturer_part_number\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_PriceComparison",
                table: "Offer",
                columns: new[] { "component_id", "currency", "price" },
                filter: "\"is_available\" IS TRUE");

            migrationBuilder.CreateIndex(
                name: "UX_Offer_Retailer_ExternalProduct",
                table: "Offer",
                columns: new[] { "retailer_id", "external_product_id" },
                unique: true,
                filter: "\"external_product_id\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_Active_Component",
                table: "PriceAlert",
                columns: new[] { "component_id", "currency", "target_price" },
                filter: "\"is_active\" IS TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_Offer_RecordedAt",
                table: "PriceHistory",
                columns: new[] { "offer_id", "recorded_at" },
                descending: new[] { false, true });

            // Enforce case-insensitive uniqueness for user email addresses.
            migrationBuilder.Sql(
                "CREATE UNIQUE INDEX \"UX_User_Email\" ON \"User\" (LOWER(\"email\"));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP INDEX IF EXISTS \"UX_User_Email\";");

            migrationBuilder.DropTable(
                name: "AffiliateConversion");

            migrationBuilder.DropTable(
                name: "CrawledListing");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PCBuildItem");

            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropTable(
                name: "SponsoredListing");

            migrationBuilder.DropTable(
                name: "MagicLinkClick");

            migrationBuilder.DropTable(
                name: "CrawlRun");

            migrationBuilder.DropTable(
                name: "PriceAlert");

            migrationBuilder.DropTable(
                name: "MagicLink");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "PCBuild");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Retailer");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
