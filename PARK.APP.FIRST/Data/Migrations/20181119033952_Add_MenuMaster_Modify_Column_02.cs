﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PARK.APP.FIRST.Data.Migrations
{
    public partial class Add_MenuMaster_Modify_Column_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuMaster",
                columns: table => new
                {
                    MenuIdentity = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuId = table.Column<string>(nullable: true),
                    MenuName = table.Column<string>(nullable: true),
                    Parent_MenuId = table.Column<string>(nullable: true),
                    User_Roll = table.Column<string>(nullable: true),
                    User_Auth = table.Column<string>(nullable: true),
                    User_Duty = table.Column<string>(nullable: true),
                    MenuArea = table.Column<string>(nullable: true),
                    MenuController = table.Column<string>(nullable: true),
                    MenuAction = table.Column<string>(nullable: true),
                    Use_Yn = table.Column<string>(nullable: true),
                    Sort_Order = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuMaster", x => x.MenuIdentity);
                });

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "MenuMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "MenuMaster");

            migrationBuilder.DropTable(
                name: "MenuMaster");
        }
    }
}
