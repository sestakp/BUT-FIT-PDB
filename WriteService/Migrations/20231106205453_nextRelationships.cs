using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WriteService.Migrations;

/// <inheritdoc />
public partial class nextRelationships : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Customers_CustomerEntityId",
            table: "Orders");

        migrationBuilder.DropIndex(
            name: "IX_Orders_CustomerEntityId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "CustomerEntityId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "Price",
            table: "Orders");

        migrationBuilder.AddColumn<long>(
            name: "CustomerId",
            table: "Orders",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Country = table.Column<string>(type: "text", nullable: false),
                ZipCode = table.Column<string>(type: "text", nullable: false),
                City = table.Column<string>(type: "text", nullable: false),
                Street = table.Column<string>(type: "text", nullable: false),
                HouseNumber = table.Column<string>(type: "text", nullable: false),
                CustomerId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Addresses_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CustomerId",
            table: "Orders",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_CustomerId",
            table: "Addresses",
            column: "CustomerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders");

        migrationBuilder.DropTable(
            name: "Addresses");

        migrationBuilder.DropIndex(
            name: "IX_Orders_CustomerId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "CustomerId",
            table: "Orders");

        migrationBuilder.AddColumn<long>(
            name: "CustomerEntityId",
            table: "Orders",
            type: "bigint",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "Price",
            table: "Orders",
            type: "numeric",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CustomerEntityId",
            table: "Orders",
            column: "CustomerEntityId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Customers_CustomerEntityId",
            table: "Orders",
            column: "CustomerEntityId",
            principalTable: "Customers",
            principalColumn: "Id");
    }
}