using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WriteService.Migrations;

/// <inheritdoc />
public partial class addAddress : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AddressEntity_Customers_CustomerEntityId",
            table: "AddressEntity");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AddressEntity",
            table: "AddressEntity");

        migrationBuilder.RenameTable(
            name: "AddressEntity",
            newName: "Addresses");

        migrationBuilder.RenameIndex(
            name: "IX_AddressEntity_CustomerEntityId",
            table: "Addresses",
            newName: "IX_Addresses_CustomerEntityId");

        migrationBuilder.AddColumn<long>(
            name: "CustomerId",
            table: "Addresses",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Addresses",
            table: "Addresses",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_CustomerId",
            table: "Addresses",
            column: "CustomerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Addresses_Customers_CustomerEntityId",
            table: "Addresses",
            column: "CustomerEntityId",
            principalTable: "Customers",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Addresses_Customers_CustomerId",
            table: "Addresses",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Addresses_Customers_CustomerEntityId",
            table: "Addresses");

        migrationBuilder.DropForeignKey(
            name: "FK_Addresses_Customers_CustomerId",
            table: "Addresses");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Addresses",
            table: "Addresses");

        migrationBuilder.DropIndex(
            name: "IX_Addresses_CustomerId",
            table: "Addresses");

        migrationBuilder.DropColumn(
            name: "CustomerId",
            table: "Addresses");

        migrationBuilder.RenameTable(
            name: "Addresses",
            newName: "AddressEntity");

        migrationBuilder.RenameIndex(
            name: "IX_Addresses_CustomerEntityId",
            table: "AddressEntity",
            newName: "IX_AddressEntity_CustomerEntityId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AddressEntity",
            table: "AddressEntity",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_AddressEntity_Customers_CustomerEntityId",
            table: "AddressEntity",
            column: "CustomerEntityId",
            principalTable: "Customers",
            principalColumn: "Id");
    }
}