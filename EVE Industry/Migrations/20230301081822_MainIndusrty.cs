using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EVE_Industry.Migrations
{
    public partial class MainIndusrty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainIndustryCells",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeItem = table.Column<int>(type: "int", nullable: false),
                    MaterialEfficiency = table.Column<int>(type: "int", nullable: false),
                    TimeEfficiency = table.Column<int>(type: "int", nullable: false),
                    ProfitPerHour = table.Column<long>(type: "bigint", nullable: false),
                    ManufacturingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Profit = table.Column<long>(type: "bigint", nullable: false),
                    typeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainIndustryCells", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainIndustryCells");
        }
    }
}
