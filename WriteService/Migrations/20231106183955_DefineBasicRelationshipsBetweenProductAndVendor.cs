using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WriteService.Migrations
{
    /// <inheritdoc />
    public partial class DefineBasicRelationshipsBetweenProductAndVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductEntityId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductEntityId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "ProductReviews");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductReviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductReviews");

            migrationBuilder.AddColumn<long>(
                name: "ProductEntityId",
                table: "ProductReviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductEntityId",
                table: "ProductReviews",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductEntityId",
                table: "ProductReviews",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
