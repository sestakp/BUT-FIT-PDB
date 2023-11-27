using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WriteService.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryEntityId",
                table: "ProductSubCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductEntityId",
                table: "ProductSubCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductCategoryEntityId",
                table: "ProductSubCategories",
                column: "ProductCategoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductEntityId",
                table: "ProductSubCategories",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryEntit~",
                table: "ProductSubCategories",
                column: "ProductCategoryEntityId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_Products_ProductEntityId",
                table: "ProductSubCategories",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryEntit~",
                table: "ProductSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_Products_ProductEntityId",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_ProductCategoryEntityId",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_ProductEntityId",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryEntityId",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "ProductSubCategories");
        }
    }
}
