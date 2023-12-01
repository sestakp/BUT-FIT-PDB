using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WriteService.Migrations
{
    /// <inheritdoc />
    public partial class removeRelationshipFromAddressToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Addresses");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "ProductSubCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryEntityId1",
                table: "ProductSubCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerEntityId1",
                table: "Addresses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_CategoryId",
                table: "ProductSubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductCategoryEntityId1",
                table: "ProductSubCategories",
                column: "ProductCategoryEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerEntityId1",
                table: "Addresses",
                column: "CustomerEntityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerEntityId1",
                table: "Addresses",
                column: "CustomerEntityId1",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_CategoryId",
                table: "ProductSubCategories",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryEnti~1",
                table: "ProductSubCategories",
                column: "ProductCategoryEntityId1",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerEntityId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_CategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryEnti~1",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_CategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_ProductCategoryEntityId1",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerEntityId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryEntityId1",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId1",
                table: "Addresses");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Addresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
