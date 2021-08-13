using Microsoft.EntityFrameworkCore.Migrations;

namespace DictionaryBack.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Term = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Translation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Term);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
