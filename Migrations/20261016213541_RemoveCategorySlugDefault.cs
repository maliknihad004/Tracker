using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategorySlugDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The model snapshot already has no default; correct the live schema explicitly.
            migrationBuilder.Sql(
                "ALTER TABLE \"Category\" ALTER COLUMN \"slug\" DROP DEFAULT;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Category\" ALTER COLUMN \"slug\" SET DEFAULT '';");
        }
    }
}
