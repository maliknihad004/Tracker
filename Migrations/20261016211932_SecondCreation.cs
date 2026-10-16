using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tracker.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffiliateConversion_Click",
                table: "AffiliateConversion");

            migrationBuilder.DropForeignKey(
                name: "FK_AffiliateConversion_Retailer",
                table: "AffiliateConversion");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawledListing_Component",
                table: "CrawledListing");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawlRun_Retailer",
                table: "CrawlRun");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Component",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Retailer",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceAlert_Component",
                table: "PriceAlert");

            migrationBuilder.DropTable(
                name: "MagicLinkClick");

            migrationBuilder.DropTable(
                name: "PCBuildItem");

            migrationBuilder.DropTable(
                name: "Retailer");

            migrationBuilder.DropTable(
                name: "MagicLink");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "PCBuild");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Name",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SponsoredListing_Currency",
                table: "SponsoredListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SponsoredListing_Dates",
                table: "SponsoredListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SponsoredListing_Fee",
                table: "SponsoredListing");

            migrationBuilder.DropIndex(
                name: "IX_PriceHistory_Offer_RecordedAt",
                table: "PriceHistory");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceHistory_Currency",
                table: "PriceHistory");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceHistory_Price",
                table: "PriceHistory");

            migrationBuilder.DropIndex(
                name: "IX_PriceAlert_Active_Component",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceAlert_Currency",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceAlert_LastTriggered",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceAlert_TargetPrice",
                table: "PriceAlert");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_Offer_Retailer_ProductUrl",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_PriceComparison",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "UX_Offer_Retailer_ExternalProduct",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Currency",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Price",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_ShippingCost",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Notification_Message",
                table: "Notification");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Notification_Title",
                table: "Notification");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_Completion",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_Dates",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_ProductsFound",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_Status",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_Currency",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_MatchedComponent",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_Price",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_PriceCurrency",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_Specifications",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawledListing_Status",
                table: "CrawledListing");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_Category_Name",
                table: "Category");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Category_Name",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "UX_AffiliateConversion_External",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffiliateConversion_Commission",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffiliateConversion_Currency",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffiliateConversion_OrderAmount",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffiliateConversion_Status",
                table: "AffiliateConversion");

            migrationBuilder.DropColumn(
                name: "external_product_id",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "status",
                table: "CrawlRun");

            migrationBuilder.DropColumn(
                name: "external_product_id",
                table: "CrawledListing");

            migrationBuilder.DropColumn(
                name: "match_status",
                table: "CrawledListing");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "status",
                table: "AffiliateConversion");

            migrationBuilder.RenameColumn(
                name: "component_id",
                table: "PriceAlert",
                newName: "variant_id");

            migrationBuilder.RenameColumn(
                name: "retailer_id",
                table: "Offer",
                newName: "variant_id");

            migrationBuilder.RenameColumn(
                name: "component_id",
                table: "Offer",
                newName: "merchant_id");

            migrationBuilder.RenameColumn(
                name: "retailer_id",
                table: "CrawlRun",
                newName: "merchant_id");

            migrationBuilder.RenameColumn(
                name: "component_id",
                table: "CrawledListing",
                newName: "matched_variant_id");

            migrationBuilder.RenameColumn(
                name: "retailer_id",
                table: "AffiliateConversion",
                newName: "merchant_id");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AddColumn<string>(
                name: "preferred_language_code",
                table: "User",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "recorded_at",
                table: "PriceHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "PriceAlert",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AddColumn<JsonDocument>(
                name: "attributes",
                table: "Offer",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "external_offer_id",
                table: "Offer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "Notification",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AddColumn<string>(
                name: "status_code",
                table: "CrawlRun",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "pending");

            migrationBuilder.AddColumn<string>(
                name: "status_scope",
                table: "CrawlRun",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "crawl_run");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "crawled_at",
                table: "CrawledListing",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AddColumn<string>(
                name: "external_offer_id",
                table: "CrawledListing",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status_code",
                table: "CrawledListing",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "pending");

            migrationBuilder.AddColumn<string>(
                name: "status_scope",
                table: "CrawledListing",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "listing_match");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_category_id",
                table: "Category",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "Category",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "reported_at",
                table: "AffiliateConversion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "status_code",
                table: "AffiliateConversion",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "pending");

            migrationBuilder.AddColumn<string>(
                name: "status_scope",
                table: "AffiliateConversion",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "affiliate_conversion");

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
                name: "IX_User_preferred_language_code",
                table: "User",
                column: "preferred_language_code");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email",
                table: "User",
                sql: "length(trim(email)) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Name",
                table: "User",
                sql: "length(trim(name)) > 0");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredListing_offer_id",
                table: "SponsoredListing",
                column: "offer_id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Sponsored_Currency",
                table: "SponsoredListing",
                sql: "currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Sponsored_Dates",
                table: "SponsoredListing",
                sql: "end_date > start_date");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Sponsored_Fee",
                table: "SponsoredListing",
                sql: "sponsorship_fee IS NULL OR sponsorship_fee >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_Offer_Date",
                table: "PriceHistory",
                columns: new[] { "offer_id", "recorded_at" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceHistory_Currency",
                table: "PriceHistory",
                sql: "currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceHistory_Price",
                table: "PriceHistory",
                sql: "price > 0");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_user_id",
                table: "PriceAlert",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_Variant_Currency",
                table: "PriceAlert",
                columns: new[] { "variant_id", "currency", "target_price" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Alert_Currency",
                table: "PriceAlert",
                sql: "currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Alert_LastTriggered",
                table: "PriceAlert",
                sql: "last_triggered_at IS NULL OR last_triggered_at >= created_at");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Alert_TargetPrice",
                table: "PriceAlert",
                sql: "target_price > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_PriceComparison",
                table: "Offer",
                columns: new[] { "variant_id", "currency", "price" });

            migrationBuilder.CreateIndex(
                name: "UX_Offer_Merchant_Listing_Variant",
                table: "Offer",
                columns: new[] { "merchant_id", "external_offer_id", "variant_id" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Attributes",
                table: "Offer",
                sql: "attributes IS NULL OR jsonb_typeof(attributes) = 'object'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Currency",
                table: "Offer",
                sql: "currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Price",
                table: "Offer",
                sql: "price > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_ProductUrl",
                table: "Offer",
                sql: "length(trim(product_url)) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Shipping",
                table: "Offer",
                sql: "shipping_cost IS NULL OR shipping_cost >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_alert_id",
                table: "Notification",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_user_id",
                table: "Notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_CrawlRun_merchant_id",
                table: "CrawlRun",
                column: "merchant_id");

            migrationBuilder.CreateIndex(
                name: "IX_CrawlRun_status_scope_status_code",
                table: "CrawlRun",
                columns: new[] { "status_scope", "status_code" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_Dates",
                table: "CrawlRun",
                sql: "finished_at IS NULL OR (started_at IS NOT NULL AND finished_at >= started_at)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_ProductsFound",
                table: "CrawlRun",
                sql: "products_found >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_StatusScope",
                table: "CrawlRun",
                sql: "status_scope = 'crawl_run'");

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Currency",
                table: "CrawledListing",
                sql: "currency IS NULL OR currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Price",
                table: "CrawledListing",
                sql: "price IS NULL OR price > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_PriceCurrency",
                table: "CrawledListing",
                sql: "price IS NULL OR currency IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_Specifications",
                table: "CrawledListing",
                sql: "raw_specifications IS NULL OR jsonb_typeof(raw_specifications) = 'object'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Listing_StatusScope",
                table: "CrawledListing",
                sql: "status_scope = 'listing_match'");

            migrationBuilder.CreateIndex(
                name: "IX_Category_parent_category_id",
                table: "Category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Category_Slug",
                table: "Category",
                column: "slug",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Category_NotOwnParent",
                table: "Category",
                sql: "parent_category_id IS NULL OR parent_category_id <> category_id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Category_Slug",
                table: "Category",
                sql: "length(trim(slug)) > 0");

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Conversion_Commission",
                table: "AffiliateConversion",
                sql: "commission_amount >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Conversion_Currency",
                table: "AffiliateConversion",
                sql: "currency ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Conversion_OrderAmount",
                table: "AffiliateConversion",
                sql: "order_amount IS NULL OR order_amount >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Conversion_StatusScope",
                table: "AffiliateConversion",
                sql: "status_scope = 'affiliate_conversion'");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslation_language_code",
                table: "CategoryTranslation",
                column: "language_code");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliateConversion_Merchant",
                table: "AffiliateConversion",
                column: "merchant_id",
                principalTable: "Merchant",
                principalColumn: "merchant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliateConversion_ShareLinkClick",
                table: "AffiliateConversion",
                column: "click_id",
                principalTable: "ShareLinkClick",
                principalColumn: "click_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliateConversion_Status",
                table: "AffiliateConversion",
                columns: new[] { "status_scope", "status_code" },
                principalTable: "Status",
                principalColumns: new[] { "scope", "code" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category",
                table: "Category",
                column: "parent_category_id",
                principalTable: "Category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawledListing_ProductVariant",
                table: "CrawledListing",
                column: "matched_variant_id",
                principalTable: "ProductVariant",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawledListing_Status",
                table: "CrawledListing",
                columns: new[] { "status_scope", "status_code" },
                principalTable: "Status",
                principalColumns: new[] { "scope", "code" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawlRun_Merchant",
                table: "CrawlRun",
                column: "merchant_id",
                principalTable: "Merchant",
                principalColumn: "merchant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawlRun_Status",
                table: "CrawlRun",
                columns: new[] { "status_scope", "status_code" },
                principalTable: "Status",
                principalColumns: new[] { "scope", "code" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Merchant",
                table: "Offer",
                column: "merchant_id",
                principalTable: "Merchant",
                principalColumn: "merchant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_ProductVariant",
                table: "Offer",
                column: "variant_id",
                principalTable: "ProductVariant",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceAlert_ProductVariant",
                table: "PriceAlert",
                column: "variant_id",
                principalTable: "ProductVariant",
                principalColumn: "variant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Language",
                table: "User",
                column: "preferred_language_code",
                principalTable: "Language",
                principalColumn: "language_code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffiliateConversion_Merchant",
                table: "AffiliateConversion");

            migrationBuilder.DropForeignKey(
                name: "FK_AffiliateConversion_ShareLinkClick",
                table: "AffiliateConversion");

            migrationBuilder.DropForeignKey(
                name: "FK_AffiliateConversion_Status",
                table: "AffiliateConversion");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawledListing_ProductVariant",
                table: "CrawledListing");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawledListing_Status",
                table: "CrawledListing");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawlRun_Merchant",
                table: "CrawlRun");

            migrationBuilder.DropForeignKey(
                name: "FK_CrawlRun_Status",
                table: "CrawlRun");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Merchant",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_ProductVariant",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceAlert_ProductVariant",
                table: "PriceAlert");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Language",
                table: "User");

            migrationBuilder.DropTable(
                name: "CategoryTranslation");

            migrationBuilder.DropTable(
                name: "Merchant");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductTranslation");

            migrationBuilder.DropTable(
                name: "SavedCollectionItem");

            migrationBuilder.DropTable(
                name: "ShareLinkClick");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropTable(
                name: "ShareLink");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SavedCollection");

            migrationBuilder.DropIndex(
                name: "IX_User_preferred_language_code",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Name",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_SponsoredListing_offer_id",
                table: "SponsoredListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Sponsored_Currency",
                table: "SponsoredListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Sponsored_Dates",
                table: "SponsoredListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Sponsored_Fee",
                table: "SponsoredListing");

            migrationBuilder.DropIndex(
                name: "IX_PriceHistory_Offer_Date",
                table: "PriceHistory");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceHistory_Currency",
                table: "PriceHistory");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PriceHistory_Price",
                table: "PriceHistory");

            migrationBuilder.DropIndex(
                name: "IX_PriceAlert_user_id",
                table: "PriceAlert");

            migrationBuilder.DropIndex(
                name: "IX_PriceAlert_Variant_Currency",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Alert_Currency",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Alert_LastTriggered",
                table: "PriceAlert");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Alert_TargetPrice",
                table: "PriceAlert");

            migrationBuilder.DropIndex(
                name: "IX_Offer_PriceComparison",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "UX_Offer_Merchant_Listing_Variant",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Attributes",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Currency",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Price",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_ProductUrl",
                table: "Offer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Offer_Shipping",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Notification_alert_id",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_user_id",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_CrawlRun_merchant_id",
                table: "CrawlRun");

            migrationBuilder.DropIndex(
                name: "IX_CrawlRun_status_scope_status_code",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_Dates",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_ProductsFound",
                table: "CrawlRun");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CrawlRun_StatusScope",
                table: "CrawlRun");

            migrationBuilder.DropIndex(
                name: "IX_CrawledListing_crawl_run_id",
                table: "CrawledListing");

            migrationBuilder.DropIndex(
                name: "IX_CrawledListing_matched_variant_id",
                table: "CrawledListing");

            migrationBuilder.DropIndex(
                name: "IX_CrawledListing_status_scope_status_code",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Currency",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Price",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_PriceCurrency",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_Specifications",
                table: "CrawledListing");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Listing_StatusScope",
                table: "CrawledListing");

            migrationBuilder.DropIndex(
                name: "IX_Category_parent_category_id",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "UQ_Category_Slug",
                table: "Category");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Category_NotOwnParent",
                table: "Category");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Category_Slug",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_AffiliateConversion_click_id",
                table: "AffiliateConversion");

            migrationBuilder.DropIndex(
                name: "IX_AffiliateConversion_offer_id",
                table: "AffiliateConversion");

            migrationBuilder.DropIndex(
                name: "IX_AffiliateConversion_status_scope_status_code",
                table: "AffiliateConversion");

            migrationBuilder.DropIndex(
                name: "UX_Conversion_Merchant_ExternalId",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Conversion_Commission",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Conversion_Currency",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Conversion_OrderAmount",
                table: "AffiliateConversion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Conversion_StatusScope",
                table: "AffiliateConversion");

            migrationBuilder.DropColumn(
                name: "preferred_language_code",
                table: "User");

            migrationBuilder.DropColumn(
                name: "attributes",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "external_offer_id",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "status_code",
                table: "CrawlRun");

            migrationBuilder.DropColumn(
                name: "status_scope",
                table: "CrawlRun");

            migrationBuilder.DropColumn(
                name: "external_offer_id",
                table: "CrawledListing");

            migrationBuilder.DropColumn(
                name: "status_code",
                table: "CrawledListing");

            migrationBuilder.DropColumn(
                name: "status_scope",
                table: "CrawledListing");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "parent_category_id",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "status_code",
                table: "AffiliateConversion");

            migrationBuilder.DropColumn(
                name: "status_scope",
                table: "AffiliateConversion");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "PriceAlert",
                newName: "component_id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "Offer",
                newName: "retailer_id");

            migrationBuilder.RenameColumn(
                name: "merchant_id",
                table: "Offer",
                newName: "component_id");

            migrationBuilder.RenameColumn(
                name: "merchant_id",
                table: "CrawlRun",
                newName: "retailer_id");

            migrationBuilder.RenameColumn(
                name: "matched_variant_id",
                table: "CrawledListing",
                newName: "component_id");

            migrationBuilder.RenameColumn(
                name: "merchant_id",
                table: "AffiliateConversion",
                newName: "retailer_id");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "recorded_at",
                table: "PriceHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "PriceAlert",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "external_product_id",
                table: "Offer",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "Notification",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "CrawlRun",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "crawled_at",
                table: "CrawledListing",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "external_product_id",
                table: "CrawledListing",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "match_status",
                table: "CrawledListing",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Category",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Category",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "reported_at",
                table: "AffiliateConversion",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "AffiliateConversion",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_Offer_Retailer_ProductUrl",
                table: "Offer",
                columns: new[] { "retailer_id", "product_url" });

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_Category_Name",
                table: "Category",
                column: "name");

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    component_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    brand = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    manufacturer_part_number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    specifications = table.Column<string>(type: "jsonb", nullable: true)
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
                name: "PCBuild",
                columns: table => new
                {
                    build_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    budget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                name: "Retailer",
                columns: table => new
                {
                    retailer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    supports_affiliate_links = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    website_url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailer", x => x.retailer_id);
                    table.UniqueConstraint("UQ_Retailer_Website", x => x.website_url);
                    table.CheckConstraint("CK_Retailer_Name", "LENGTH(TRIM(\"name\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "MagicLink",
                columns: table => new
                {
                    magic_link_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    build_id = table.Column<int>(type: "integer", nullable: true),
                    component_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
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

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email",
                table: "User",
                sql: "LENGTH(TRIM(\"email\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Name",
                table: "User",
                sql: "LENGTH(TRIM(\"name\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SponsoredListing_Currency",
                table: "SponsoredListing",
                sql: "\"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SponsoredListing_Dates",
                table: "SponsoredListing",
                sql: "\"end_date\" > \"start_date\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SponsoredListing_Fee",
                table: "SponsoredListing",
                sql: "\"sponsorship_fee\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_Offer_RecordedAt",
                table: "PriceHistory",
                columns: new[] { "offer_id", "recorded_at" },
                descending: new[] { false, true });

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceHistory_Currency",
                table: "PriceHistory",
                sql: "\"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceHistory_Price",
                table: "PriceHistory",
                sql: "\"price\" > 0");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAlert_Active_Component",
                table: "PriceAlert",
                columns: new[] { "component_id", "currency", "target_price" },
                filter: "\"is_active\" IS TRUE");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceAlert_Currency",
                table: "PriceAlert",
                sql: "\"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceAlert_LastTriggered",
                table: "PriceAlert",
                sql: "\"last_triggered_at\" IS NULL OR \"last_triggered_at\" >= \"created_at\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PriceAlert_TargetPrice",
                table: "PriceAlert",
                sql: "\"target_price\" > 0");

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Currency",
                table: "Offer",
                sql: "\"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_Price",
                table: "Offer",
                sql: "\"price\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Offer_ShippingCost",
                table: "Offer",
                sql: "\"shipping_cost\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Notification_Message",
                table: "Notification",
                sql: "LENGTH(TRIM(\"message\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Notification_Title",
                table: "Notification",
                sql: "LENGTH(TRIM(\"title\")) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_Completion",
                table: "CrawlRun",
                sql: "(\"status\" = 'Pending' AND \"started_at\" IS NULL AND \"finished_at\" IS NULL) OR (\"status\" = 'Running' AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NULL) OR (\"status\" IN ('Completed','Failed') AND \"started_at\" IS NOT NULL AND \"finished_at\" IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_Dates",
                table: "CrawlRun",
                sql: "\"finished_at\" IS NULL OR (\"started_at\" IS NOT NULL AND \"finished_at\" >= \"started_at\")");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_ProductsFound",
                table: "CrawlRun",
                sql: "\"products_found\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawlRun_Status",
                table: "CrawlRun",
                sql: "\"status\" IN ('Pending','Running','Completed','Failed')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_Currency",
                table: "CrawledListing",
                sql: "\"currency\" IS NULL OR \"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_MatchedComponent",
                table: "CrawledListing",
                sql: "\"match_status\" <> 'Matched' OR \"component_id\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_Price",
                table: "CrawledListing",
                sql: "\"price\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_PriceCurrency",
                table: "CrawledListing",
                sql: "\"price\" IS NULL OR \"currency\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_Specifications",
                table: "CrawledListing",
                sql: "\"raw_specifications\" IS NULL OR jsonb_typeof(\"raw_specifications\") = 'object'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrawledListing_Status",
                table: "CrawledListing",
                sql: "\"match_status\" IN ('Pending','Matched','Unmatched','Review')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Category_Name",
                table: "Category",
                sql: "LENGTH(TRIM(\"name\")) > 0");

            migrationBuilder.CreateIndex(
                name: "UX_AffiliateConversion_External",
                table: "AffiliateConversion",
                columns: new[] { "retailer_id", "external_conversion_id" },
                unique: true,
                filter: "\"external_conversion_id\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffiliateConversion_Commission",
                table: "AffiliateConversion",
                sql: "\"commission_amount\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffiliateConversion_Currency",
                table: "AffiliateConversion",
                sql: "\"currency\" ~ '^[A-Z]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffiliateConversion_OrderAmount",
                table: "AffiliateConversion",
                sql: "\"order_amount\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffiliateConversion_Status",
                table: "AffiliateConversion",
                sql: "\"status\" IN ('Pending','Approved','Rejected','Paid')");

            migrationBuilder.CreateIndex(
                name: "UX_Component_Brand_MPN",
                table: "Component",
                columns: new[] { "brand", "manufacturer_part_number" },
                unique: true,
                filter: "\"manufacturer_part_number\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliateConversion_Click",
                table: "AffiliateConversion",
                column: "click_id",
                principalTable: "MagicLinkClick",
                principalColumn: "click_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AffiliateConversion_Retailer",
                table: "AffiliateConversion",
                column: "retailer_id",
                principalTable: "Retailer",
                principalColumn: "retailer_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawledListing_Component",
                table: "CrawledListing",
                column: "component_id",
                principalTable: "Component",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrawlRun_Retailer",
                table: "CrawlRun",
                column: "retailer_id",
                principalTable: "Retailer",
                principalColumn: "retailer_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Component",
                table: "Offer",
                column: "component_id",
                principalTable: "Component",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Retailer",
                table: "Offer",
                column: "retailer_id",
                principalTable: "Retailer",
                principalColumn: "retailer_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceAlert_Component",
                table: "PriceAlert",
                column: "component_id",
                principalTable: "Component",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
