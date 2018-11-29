using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PARK.APP.FIRST.Migrations.SystemDb
{
    public partial class CommonCode_Add_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TCommonCode",
                columns: table => new
                {
                    CodeType = table.Column<string>(maxLength: 32, nullable: false),
                    CodeTypeName = table.Column<string>(maxLength: 128, nullable: false),
                    CodeId = table.Column<string>(maxLength: 32, nullable: false),
                    CodeName = table.Column<string>(maxLength: 128, nullable: false),
                    CodeVal = table.Column<int>(nullable: true),
                    SortOrder = table.Column<int>(nullable: true),
                    UseYn = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    CodeComment = table.Column<string>(maxLength: 128, nullable: true),
                    CreateUserId = table.Column<string>(maxLength: 16, nullable: true),
                    CreateDt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCommonCode", x => new { x.CodeType, x.CodeId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TCommonCode");
        }
    }
}
