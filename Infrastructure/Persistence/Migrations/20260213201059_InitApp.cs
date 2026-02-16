using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_subcategory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_subcategory", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_subcategory_product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subcategory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    price_per_item = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price_per_pack = table.Column<decimal>(type: "numeric", nullable: false),
                    article = table.Column<string>(type: "text", nullable: false),
                    items_per_pack = table.Column<int>(type: "integer", nullable: false),
                    unit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products__product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_products__product_subcategory_subcategory_id",
                        column: x => x.subcategory_id,
                        principalTable: "product_subcategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "product_parameters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_parameters", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_parameters_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1f117b18-7973-40fe-b435-2302b7485441"), "футболки" },
                    { new Guid("557e5a88-f46b-4129-a889-bc6007adf22c"), "нижнее белье" },
                    { new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3"), "носки" },
                    { new Guid("91aeebc4-07ea-483a-bb96-54ec78d7993e"), "полотенца" },
                    { new Guid("9ca149ab-b4a6-4c1b-8d38-96f1cee244b3"), "перчатки" },
                    { new Guid("af93e152-d14d-470b-8c82-1e56eac15817"), "шапки" },
                    { new Guid("f171cc4f-ed02-42f8-8732-4e2b8741d08a"), "колготки" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_parameters_product_id",
                table: "product_parameters",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_subcategory_category_id",
                table: "product_subcategory",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_subcategory_id",
                table: "products",
                column: "subcategory_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_parameters");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "product_subcategory");

            migrationBuilder.DropTable(
                name: "product_categories");
        }
    }
}
