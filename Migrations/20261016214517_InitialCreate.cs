using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    parent_category_id = table.Column<int>(type: "integer", nullable: true),
                    slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.category_id);
                    table.CheckConstraint("CK_Category_NotOwnParent", "parent_category_id IS NULL OR parent_category_id <> category_id");
                    table.CheckConstraint("CK_Category_Slug", "length(trim(slug)) > 0");
                    table.ForeignKey(
                        name: "FK_Category_Category",
                        column: x => x.parent_category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    native_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.language_code);
                    table.CheckConstraint("CK_Language_NativeName", "length(trim(native_name)) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Merchant",
                columns: table => new
                {
                    merchant_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    website_url = table.Column<string>(type: "text", nullable: true),
                    supports_affiliate_links = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchant", x => x.merchant_id);
                    table.CheckConstraint("CK_Merchant_Name", "length(trim(name)) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    brand = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    specifications = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.product_id);
                    table.CheckConstraint("CK_Product_Dates", "updated_at >= created_at");
                    table.CheckConstraint("CK_Product_Specifications", "specifications IS NULL OR jsonb_typeof(specifications) = 'object'");
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    scope = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    label_en = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    label_ar = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => new { x.scope, x.code });
                    table.CheckConstraint("CK_Status_LabelAr", "length(trim(label_ar)) > 0");
                    table.CheckConstraint("CK_Status_LabelEn", "length(trim(label_en)) > 0");
                });

            migrationBuilder.CreateTable(
                name: "CategoryTranslation",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslation", x => new { x.category_id, x.language_code });
                    table.CheckConstraint("CK_CategoryTranslation_Name", "length(trim(name)) > 0");
                    table.ForeignKey(
                        name: "FK_CategoryTranslation_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTranslation_Language",
                        column: x => x.language_code,
                        principalTable: "Language",
                        principalColumn: "language_code",
                        onDelete: ReferentialAction.Restrict);
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
                    preferred_language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.CheckConstraint("CK_User_Email", "length(trim(email)) > 0");
                    table.CheckConstraint("CK_User_Name", "length(trim(name)) > 0");
                    table.ForeignKey(
                        name: "FK_User_Language",
                        column: x => x.preferred_language_code,
                        principalTable: "Language",
                        principalColumn: "language_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.product_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Category",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTranslation",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslation", x => new { x.product_id, x.language_code });
                    table.CheckConstraint("CK_ProductTranslation_Name", "length(trim(name)) > 0");
                    table.ForeignKey(
                        name: "FK_ProductTranslation_Language",
                        column: x => x.language_code,
                        principalTable: "Language",
                        principalColumn: "language_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTranslation_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    variant_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    variant_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    gtin = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    attributes = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.variant_id);
                    table.CheckConstraint("CK_Variant_Attributes", "attributes IS NULL OR jsonb_typeof(attributes) = 'object'");
                    table.CheckConstraint("CK_Variant_Code", "length(trim(variant_code)) > 0");
                    table.ForeignKey(
                        name: "FK_ProductVariant_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrawlRun",
                columns: table => new
                {
                    crawl_run_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    merchant_id = table.Column<int>(type: "integer", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    finished_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status_scope = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "crawl_run"),
                    status_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    products_found = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    error_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlRun", x => x.crawl_run_id);
                    table.CheckConstraint("CK_CrawlRun_Dates", "finished_at IS NULL OR (started_at IS NOT NULL AND finished_at >= started_at)");
                    table.CheckConstraint("CK_CrawlRun_ProductsFound", "products_found >= 0");
                    table.CheckConstraint("CK_CrawlRun_StatusScope", "status_scope = 'crawl_run'");
                    table.ForeignKey(
                        name: "FK_CrawlRun_Merchant",
                        column: x => x.merchant_id,
                        principalTable: "Merchant",
                        principalColumn: "merchant_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrawlRun_Status",
                        columns: x => new { x.status_scope, x.status_code },
                        principalTable: "Status",
                        principalColumns: new[] { "scope", "code" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedCollection",
                columns: table => new
                {
                    collection_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedCollection", x => x.collection_id);
                    table.CheckConstraint("CK_Collection_Dates", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_SavedCollection_User",
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
                    merchant_id = table.Column<int>(type: "integer", nullable: false),
                    variant_id = table.Column<int>(type: "integer", nullable: false),
                    external_offer_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    product_url = table.Column<string>(type: "text", nullable: false),
                    affiliate_url = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    shipping_cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: true),
                    attributes = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    last_checked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.offer_id);
                    table.CheckConstraint("CK_Offer_Attributes", "attributes IS NULL OR jsonb_typeof(attributes) = 'object'");
                    table.CheckConstraint("CK_Offer_Currency", "currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Offer_Price", "price > 0");
                    table.CheckConstraint("CK_Offer_ProductUrl", "length(trim(product_url)) > 0");
                    table.CheckConstraint("CK_Offer_Shipping", "shipping_cost IS NULL OR shipping_cost >= 0");
                    table.ForeignKey(
                        name: "FK_Offer_Merchant",
                        column: x => x.merchant_id,
                        principalTable: "Merchant",
                        principalColumn: "merchant_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offer_ProductVariant",
                        column: x => x.variant_id,
                        principalTable: "ProductVariant",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceAlert",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    variant_id = table.Column<int>(type: "integer", nullable: false),
                    target_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    last_triggered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAlert", x => x.alert_id);
                    table.CheckConstraint("CK_Alert_Currency", "currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Alert_LastTriggered", "last_triggered_at IS NULL OR last_triggered_at >= created_at");
                    table.CheckConstraint("CK_Alert_TargetPrice", "target_price > 0");
                    table.ForeignKey(
                        name: "FK_PriceAlert_ProductVariant",
                        column: x => x.variant_id,
                        principalTable: "ProductVariant",
                        principalColumn: "variant_id",
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
                    matched_variant_id = table.Column<int>(type: "integer", nullable: true),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    external_offer_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    product_url = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    raw_specifications = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    status_scope = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "listing_match"),
                    status_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    crawled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawledListing", x => x.listing_id);
                    table.CheckConstraint("CK_Listing_Currency", "currency IS NULL OR currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Listing_Price", "price IS NULL OR price > 0");
                    table.CheckConstraint("CK_Listing_PriceCurrency", "price IS NULL OR currency IS NOT NULL");
                    table.CheckConstraint("CK_Listing_Specifications", "raw_specifications IS NULL OR jsonb_typeof(raw_specifications) = 'object'");
                    table.CheckConstraint("CK_Listing_StatusScope", "status_scope = 'listing_match'");
                    table.ForeignKey(
                        name: "FK_CrawledListing_CrawlRun",
                        column: x => x.crawl_run_id,
                        principalTable: "CrawlRun",
                        principalColumn: "crawl_run_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrawledListing_ProductVariant",
                        column: x => x.matched_variant_id,
                        principalTable: "ProductVariant",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CrawledListing_Status",
                        columns: x => new { x.status_scope, x.status_code },
                        principalTable: "Status",
                        principalColumns: new[] { "scope", "code" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedCollectionItem",
                columns: table => new
                {
                    collection_item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    collection_id = table.Column<int>(type: "integer", nullable: false),
                    variant_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedCollectionItem", x => x.collection_item_id);
                    table.CheckConstraint("CK_CollectionItem_Quantity", "quantity > 0");
                    table.ForeignKey(
                        name: "FK_SavedCollectionItem_ProductVariant",
                        column: x => x.variant_id,
                        principalTable: "ProductVariant",
                        principalColumn: "variant_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedCollectionItem_SavedCollection",
                        column: x => x.collection_id,
                        principalTable: "SavedCollection",
                        principalColumn: "collection_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShareLink",
                columns: table => new
                {
                    share_link_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    collection_id = table.Column<int>(type: "integer", nullable: true),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareLink", x => x.share_link_id);
                    table.CheckConstraint("CK_ShareLink_OneTarget", "(product_id IS NOT NULL AND collection_id IS NULL) OR (product_id IS NULL AND collection_id IS NOT NULL)");
                    table.CheckConstraint("CK_ShareLink_Slug", "length(trim(slug)) > 0");
                    table.ForeignKey(
                        name: "FK_ShareLink_Product",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareLink_SavedCollection",
                        column: x => x.collection_id,
                        principalTable: "SavedCollection",
                        principalColumn: "collection_id",
                        onDelete: ReferentialAction.Restrict);
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
                    recorded_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistory", x => x.price_history_id);
                    table.CheckConstraint("CK_PriceHistory_Currency", "currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_PriceHistory_Price", "price > 0");
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
                    table.CheckConstraint("CK_Sponsored_Currency", "currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Sponsored_Dates", "end_date > start_date");
                    table.CheckConstraint("CK_Sponsored_Fee", "sponsorship_fee IS NULL OR sponsorship_fee >= 0");
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
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.notification_id);
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
                name: "ShareLinkClick",
                columns: table => new
                {
                    click_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    share_link_id = table.Column<int>(type: "integer", nullable: false),
                    offer_id = table.Column<int>(type: "integer", nullable: false),
                    clicked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareLinkClick", x => x.click_id);
                    table.ForeignKey(
                        name: "FK_ShareLinkClick_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareLinkClick_ShareLink",
                        column: x => x.share_link_id,
                        principalTable: "ShareLink",
                        principalColumn: "share_link_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AffiliateConversion",
                columns: table => new
                {
                    conversion_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    merchant_id = table.Column<int>(type: "integer", nullable: false),
                    offer_id = table.Column<int>(type: "integer", nullable: true),
                    click_id = table.Column<int>(type: "integer", nullable: true),
                    external_conversion_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    order_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    commission_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    status_scope = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "affiliate_conversion"),
                    status_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    reported_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliateConversion", x => x.conversion_id);
                    table.CheckConstraint("CK_Conversion_Commission", "commission_amount >= 0");
                    table.CheckConstraint("CK_Conversion_Currency", "currency ~ '^[A-Z]{3}$'");
                    table.CheckConstraint("CK_Conversion_OrderAmount", "order_amount IS NULL OR order_amount >= 0");
                    table.CheckConstraint("CK_Conversion_StatusScope", "status_scope = 'affiliate_conversion'");
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Merchant",
                        column: x => x.merchant_id,
                        principalTable: "Merchant",
                        principalColumn: "merchant_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Offer",
                        column: x => x.offer_id,
                        principalTable: "Offer",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_ShareLinkClick",
                        column: x => x.click_id,
                        principalTable: "ShareLinkClick",
                        principalColumn: "click_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AffiliateConversion_Status",
                        columns: x => new { x.status_scope, x.status_code },
                        principalTable: "Status",
                        principalColumns: new[] { "scope", "code" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "language_code", "is_active", "native_name" },
                values: new object[,]
                {
                    { "ar", true, "العربية" },
                    { "en", true, "English" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en", "sort_order" },
                values: new object[,]
                {
                    { "approved", "affiliate_conversion", true, "تمت الموافقة", "Approved", 10 },
                    { "paid", "affiliate_conversion", true, "مدفوع", "Paid", 30 }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en" },
                values: new object[] { "pending", "affiliate_conversion", true, "قيد الانتظار", "Pending" });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en", "sort_order" },
                values: new object[,]
                {
                    { "rejected", "affiliate_conversion", true, "مرفوض", "Rejected", 20 },
                    { "completed", "crawl_run", true, "مكتمل", "Completed", 20 },
                    { "failed", "crawl_run", true, "فشل", "Failed", 30 }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en" },
                values: new object[] { "pending", "crawl_run", true, "قيد الانتظار", "Pending" });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en", "sort_order" },
                values: new object[,]
                {
                    { "running", "crawl_run", true, "قيد التنفيذ", "Running", 10 },
                    { "matched", "listing_match", true, "مطابق", "Matched", 10 }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en" },
                values: new object[] { "pending", "listing_match", true, "قيد الانتظار", "Pending" });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "code", "scope", "is_active", "label_ar", "label_en", "sort_order" },
                values: new object[,]
                {
                    { "review", "listing_match", true, "يحتاج مراجعة", "Needs review", 30 },
                    { "unmatched", "listing_match", true, "غير مطابق", "Unmatched", 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateConversion_click_id",
                table: "AffiliateConversion",
                column: "click_id");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateConversion_offer_id",
                table: "AffiliateConversion",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateConversion_status_scope_status_code",
                table: "AffiliateConversion",
                columns: new[] { "status_scope", "status_code" });

            migrationBuilder.CreateIndex(
                name: "UX_Conversion_Merchant_ExternalId",
                table: "AffiliateConversion",
                columns: new[] { "merchant_id", "external_conversion_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_parent_category_id",
                table: "Category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Category_Slug",
                table: "Category",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslation_language_code",
                table: "CategoryTranslation",
                column: "language_code");

            migrationBuilder.CreateIndex(
                name: "IX_CrawledListing_crawl_run_id",
                table: "CrawledListing",
                column: "crawl_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_CrawledListing_matched_variant_id",
                table: "CrawledListing",
                column: "matched_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_CrawledListing_status_scope_status_code",
                table: "CrawledListing",
                columns: new[] { "status_scope", "status_code" });

            migrationBuilder.CreateIndex(
                name: "IX_CrawlRun_merchant_id",
                table: "CrawlRun",
                column: "merchant_id");

            migrationBuilder.CreateIndex(
                name: "IX_CrawlRun_status_scope_status_code",
                table: "CrawlRun",
                columns: new[] { "status_scope", "status_code" });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_alert_id",
                table: "Notification",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_user_id",
                table: "Notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_PriceComparison",
                table: "Offer",
                columns: new[] { "variant_id", "currency", "price" });

            migrationBuilder.CreateIndex(
                name: "UX_Offer_Merchant_Listing_Variant",
                table: "Offer",
                columns: new[] { "merchant_id", "external_offer_id", "variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_user_id",
                table: "PriceAlert",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_Variant_Currency",
                table: "PriceAlert",
                columns: new[] { "variant_id", "currency", "target_price" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_Offer_Date",
                table: "PriceHistory",
                columns: new[] { "offer_id", "recorded_at" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_category_id",
                table: "ProductCategory",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslation_language_code",
                table: "ProductTranslation",
                column: "language_code");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductVariant_Gtin",
                table: "ProductVariant",
                column: "gtin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Variant_ProductCode",
                table: "ProductVariant",
                columns: new[] { "product_id", "variant_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedCollection_user_id",
                table: "SavedCollection",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SavedCollectionItem_variant_id",
                table: "SavedCollectionItem",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Collection_Variant",
                table: "SavedCollectionItem",
                columns: new[] { "collection_id", "variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareLink_collection_id",
                table: "ShareLink",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShareLink_product_id",
                table: "ShareLink",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "UQ_ShareLink_Slug",
                table: "ShareLink",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareLinkClick_offer_id",
                table: "ShareLinkClick",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShareLinkClick_share_link_id",
                table: "ShareLinkClick",
                column: "share_link_id");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredListing_offer_id",
                table: "SponsoredListing",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_preferred_language_code",
                table: "User",
                column: "preferred_language_code");

            // Expression indexes are not represented in the EF Core model snapshot.
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
                name: "CategoryTranslation");

            migrationBuilder.DropTable(
                name: "CrawledListing");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductTranslation");

            migrationBuilder.DropTable(
                name: "SavedCollectionItem");

            migrationBuilder.DropTable(
                name: "SponsoredListing");

            migrationBuilder.DropTable(
                name: "ShareLinkClick");

            migrationBuilder.DropTable(
                name: "CrawlRun");

            migrationBuilder.DropTable(
                name: "PriceAlert");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "ShareLink");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Merchant");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropTable(
                name: "SavedCollection");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
