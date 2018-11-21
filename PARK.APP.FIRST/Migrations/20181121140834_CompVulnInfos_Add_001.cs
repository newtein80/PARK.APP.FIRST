using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PARK.APP.FIRST.Migrations
{
    public partial class CompVulnInfos_Add_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TCompInfo",
                columns: table => new
                {
                    COMP_SEQ = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_NAME = table.Column<string>(maxLength: 128, nullable: false),
                    COMP_DESCRIPTION = table.Column<string>(maxLength: 1024, nullable: true),
                    COMP_REF = table.Column<string>(maxLength: 256, nullable: true),
                    STANDARD_YEAR = table.Column<string>(maxLength: 4, nullable: true),
                    DIAG_TYPE = table.Column<string>(maxLength: 32, nullable: false),
                    CONFIRM_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    USE_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    CREATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    CREATE_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    UPDATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    UPDATE_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    COMP_DETAIL_GUBUN = table.Column<string>(maxLength: 32, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCompInfo", x => x.COMP_SEQ);
                });

            migrationBuilder.CreateTable(
                name: "TVulnGroup",
                columns: table => new
                {
                    GROUP_SEQ = table.Column<long>(nullable: false),
                    UPPER_SEQ = table.Column<long>(nullable: false),
                    LEVEL = table.Column<int>(nullable: false),
                    GROUP_TYPE = table.Column<string>(maxLength: 32, nullable: false),
                    COMP_SEQ = table.Column<long>(nullable: false),
                    DIAG_KIND = table.Column<string>(maxLength: 32, nullable: true),
                    DIAG_TERM = table.Column<string>(maxLength: 32, nullable: true),
                    GROUP_ID = table.Column<string>(maxLength: 32, nullable: true),
                    GROUP_NAME = table.Column<string>(maxLength: 128, nullable: true),
                    DIAG_TOOL = table.Column<string>(maxLength: 32, nullable: true),
                    SORT_ORDER = table.Column<int>(nullable: true),
                    CREATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    CREATE_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    UPDATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    UPDATE_DT = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVulnGroup", x => x.GROUP_SEQ);
                    table.ForeignKey(
                        name: "FK__TVulnGrou__COMP___5091BB2E",
                        column: x => x.COMP_SEQ,
                        principalTable: "TCompInfo",
                        principalColumn: "COMP_SEQ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TVuln",
                columns: table => new
                {
                    VULN_SEQ = table.Column<long>(nullable: false),
                    GROUP_SEQ = table.Column<long>(nullable: false),
                    MANUAL_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    AUTO_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    MANAGE_ID = table.Column<string>(maxLength: 32, nullable: true),
                    CLIENT_STANDARD_ID = table.Column<string>(maxLength: 32, nullable: true),
                    VULN_NAME = table.Column<string>(maxLength: 128, nullable: true),
                    SORT_ORDER = table.Column<int>(nullable: true),
                    RULE_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    RATE = table.Column<string>(maxLength: 32, nullable: true),
                    SCORE = table.Column<string>(maxLength: 2, nullable: true),
                    APPLY_TIME = table.Column<string>(maxLength: 2, nullable: true),
                    DETAIL = table.Column<string>(type: "text", nullable: true),
                    DETAIL_PATH = table.Column<string>(maxLength: 256, nullable: true),
                    JUDGEMENT = table.Column<string>(maxLength: 2048, nullable: true),
                    EFFECT = table.Column<string>(maxLength: 2048, nullable: true),
                    REMEDY = table.Column<string>(type: "text", nullable: true),
                    REMEDY_PATH = table.Column<string>(maxLength: 256, nullable: true),
                    REFRRENCE = table.Column<string>(maxLength: 2048, nullable: true),
                    PARSER_CONTENTS = table.Column<string>(type: "text", nullable: true),
                    ORG_PARSER_CONTENTS = table.Column<string>(type: "text", nullable: true),
                    APPLY_TARGET = table.Column<string>(maxLength: 1024, nullable: true),
                    USE_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    EXCEPT_CD = table.Column<string>(maxLength: 2, nullable: true),
                    EXCEPT_TERM_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    EXCEPT_TERM_FR = table.Column<string>(maxLength: 8, nullable: true),
                    EXCEPT_TERM_TO = table.Column<string>(maxLength: 8, nullable: true),
                    EXCEPT_REASON = table.Column<string>(maxLength: 1024, nullable: true),
                    EXCEPT_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    EXCEPT_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    CREATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    CREATE_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    UPDATE_USER_ID = table.Column<string>(maxLength: 16, nullable: true),
                    UPDATE_DT = table.Column<DateTime>(type: "datetime", nullable: true),
                    VULGROUP = table.Column<long>(nullable: true, defaultValueSql: "((0))"),
                    VULNO = table.Column<string>(maxLength: 32, nullable: true, defaultValueSql: "('')"),
                    REMEDY_DETAIL = table.Column<string>(type: "text", nullable: true),
                    OVERVIEW = table.Column<string>(nullable: true),
                    MANAGEMENT_VULN_YN = table.Column<string>(unicode: false, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVuln", x => x.VULN_SEQ);
                    table.ForeignKey(
                        name: "FK__TVuln__GROUP_SEQ__536E27D9",
                        column: x => x.GROUP_SEQ,
                        principalTable: "TVulnGroup",
                        principalColumn: "GROUP_SEQ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TVuln_GROUP_SEQ",
                table: "TVuln",
                column: "GROUP_SEQ");

            migrationBuilder.CreateIndex(
                name: "IX_TVulnGroup_COMP_SEQ",
                table: "TVulnGroup",
                column: "COMP_SEQ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TVuln");

            migrationBuilder.DropTable(
                name: "TVulnGroup");

            migrationBuilder.DropTable(
                name: "TCompInfo");
        }
    }
}
