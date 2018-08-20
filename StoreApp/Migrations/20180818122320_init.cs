using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailsOrderID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderDetailsOrderID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderDetailsOrderID",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "UserProduct",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    ProductType = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    OrderDetailsOrderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserProduct_OrderDetails_OrderDetailsOrderID",
                        column: x => x.OrderDetailsOrderID,
                        principalTable: "OrderDetails",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProduct_OrderDetailsOrderID",
                table: "UserProduct",
                column: "OrderDetailsOrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProduct");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsOrderID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderDetailsOrderID",
                table: "Products",
                column: "OrderDetailsOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailsOrderID",
                table: "Products",
                column: "OrderDetailsOrderID",
                principalTable: "OrderDetails",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
