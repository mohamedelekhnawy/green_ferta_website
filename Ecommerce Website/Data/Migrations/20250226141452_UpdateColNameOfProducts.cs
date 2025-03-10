using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColNameOfProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvilable",
                table: "Products",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Products",
                newName: "IsAvilable");
        }
    }
}
