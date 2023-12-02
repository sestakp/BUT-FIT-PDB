using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WriteService.Migrations
{
    /// <inheritdoc />
    public partial class addNormalizeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "ProductSubCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "ProductCategories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ProductCategories");
        }
    }
}
