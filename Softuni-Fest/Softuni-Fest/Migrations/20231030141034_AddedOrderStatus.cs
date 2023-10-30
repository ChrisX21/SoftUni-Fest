using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Softuni_Fest.Migrations
{
    public partial class AddedOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "OrderOrderStatuses",
                columns: table => new 
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_OrderOrderStatuses", x => new { x.OrderId, x.OrderStatusId });
                    table.ForeignKey(
                        name: "FK_OrderOrderStatuses_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );

                    table.ForeignKey(
                        name: "FK_OrderOrderStatuses_Orderes_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrderStatuses_OrderStatusId",
                table: "OrderOrderStatuses",
                column: "OrderStatusId"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderOrderStatuses");

            migrationBuilder.DropTable(
                name: "OrderStatuses");
        }
    }
}
