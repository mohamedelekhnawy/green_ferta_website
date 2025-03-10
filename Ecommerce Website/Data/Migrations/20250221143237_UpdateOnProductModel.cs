using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
