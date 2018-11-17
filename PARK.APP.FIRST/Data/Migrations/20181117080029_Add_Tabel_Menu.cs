using Microsoft.EntityFrameworkCore.Migrations;

namespace PARK.APP.FIRST.Data.Migrations
{
    public partial class Add_Tabel_Menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MENU",
                columns: table => new
                {
                    MENU_SEQ = table.Column<int>(nullable: false),
                    MENU_NAME = table.Column<string>(maxLength: 128, nullable: true),
                    UPPER_MENU_SEQ = table.Column<int>(nullable: true),
                    PGM_ID = table.Column<string>(maxLength: 64, nullable: true),
                    MENU_DESCRIPTION = table.Column<string>(maxLength: 512, nullable: true),
                    IMAGE_PATH = table.Column<string>(maxLength: 256, nullable: true),
                    USE_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    SORT_ORDER = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MENU", x => x.MENU_SEQ);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MENU");
        }
    }
}
