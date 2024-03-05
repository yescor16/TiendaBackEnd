using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "ProductBrands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_StoreId",
                table: "ProductTypes",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_StoreId",
                table: "ProductBrands",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrands_Stores_StoreId",
                table: "ProductBrands",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_Stores_StoreId",
                table: "ProductTypes",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrands_Stores_StoreId",
                table: "ProductBrands");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_Stores_StoreId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_StoreId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrands_StoreId",
                table: "ProductBrands");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "ProductBrands");
        }
    }
}
