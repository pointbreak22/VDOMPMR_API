using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                table: "categories",
                columns: new[] { "id", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("1f117b18-7973-40fe-b435-2302b7485441"), "футболки", null },
                    { new Guid("557e5a88-f46b-4129-a889-bc6007adf22c"), "нижнее белье", null },
                    { new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3"), "носки", null },
                    { new Guid("91aeebc4-07ea-483a-bb96-54ec78d7993e"), "полотенца", null },
                    { new Guid("9ca149ab-b4a6-4c1b-8d38-96f1cee244b3"), "перчатки", null },
                    { new Guid("af93e152-d14d-470b-8c82-1e56eac15817"), "шапки", null },
                    { new Guid("f171cc4f-ed02-42f8-8732-4e2b8741d08a"), "колготки", null },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "мужские носки", new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "женские носки", new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "детские носки", new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "мужское белье", new Guid("557e5a88-f46b-4129-a889-bc6007adf22c") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "женское белье", new Guid("557e5a88-f46b-4129-a889-bc6007adf22c") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "спортивные футболки", new Guid("1f117b18-7973-40fe-b435-2302b7485441") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "повседневные футболки", new Guid("1f117b18-7973-40fe-b435-2302b7485441") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "термо носки", new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "зимние перчатки", new Guid("9ca149ab-b4a6-4c1b-8d38-96f1cee244b3") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "для бега", new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "для фитнеса", new Guid("66666666-6666-6666-6666-666666666666") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parameters_product_id",
                table: "product_parameters",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");
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
                name: "categories");
        }
    }
}
