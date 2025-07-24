using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFilterModelAsAForignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductFilterId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductFilters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFilters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductFilterId",
                table: "Products",
                column: "ProductFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductFilters_ProductFilterId",
                table: "Products",
                column: "ProductFilterId",
                principalTable: "ProductFilters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductFilters_ProductFilterId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductFilters");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductFilterId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductFilterId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
