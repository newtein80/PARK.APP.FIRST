using Microsoft.EntityFrameworkCore.Migrations;

namespace PARK.APP.FIRST.Data.Migrations
{
    public partial class Add_Role_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ManageRoleViewModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ManageRoleViewModel");
        }
    }
}
