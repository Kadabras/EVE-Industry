using Microsoft.EntityFrameworkCore.Migrations;

namespace EVE_Industry.Migrations
{
    public partial class ParseDump : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeId",
                table: "MainIndustryCells",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "MainIndustryCells",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "MainIndustryCells",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ParsedId",
                table: "MainIndustryCells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DumpCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profit = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ParsedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DumpCells", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DumpCells");

            migrationBuilder.DropColumn(
                name: "ParsedId",
                table: "MainIndustryCells");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "MainIndustryCells",
                newName: "typeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MainIndustryCells",
                newName: "id");

            migrationBuilder.AlterColumn<long>(
                name: "typeId",
                table: "MainIndustryCells",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
