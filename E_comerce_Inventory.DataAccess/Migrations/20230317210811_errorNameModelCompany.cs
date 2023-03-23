using Microsoft.EntityFrameworkCore.Migrations;

namespace E_comerce_Inventory.DataAccess.Migrations
{
    public partial class errorNameModelCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desciption",
                table: "Companies",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Companies",
                newName: "Desciption");
        }
    }
}
