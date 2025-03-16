using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekSepeti.Migrations
{
    /// <inheritdoc />
    public partial class deletingview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPersonnelPerformances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPersonnelPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryPersonnelId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPersonnelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDeliveredOrders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPersonnelPerformances", x => x.Id);
                });
        }
    }
}
