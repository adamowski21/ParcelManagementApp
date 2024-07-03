using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelManagementApp.Migrations
{
    public partial class Weight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Parcels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Parcels",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
