using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oil_System.Migrations
{
    /// <inheritdoc />
    public partial class OilPacketFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BuyingPrice",
                table: "OilPackets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "OilPackets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "OilPackets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "OilPackets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyingPrice",
                table: "OilPackets");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "OilPackets");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "OilPackets");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "OilPackets");
        }
    }
}
