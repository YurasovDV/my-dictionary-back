using Microsoft.EntityFrameworkCore.Migrations;

namespace DictionaryBack.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    term = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    topic = table.Column<string>(type: "text", nullable: true, defaultValue: "user"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.term);
                });

            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    meaning = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    term = table.Column<string>(type: "character varying(200)", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translations", x => new { x.term, x.meaning });
                    table.ForeignKey(
                        name: "FK_translations_words_term",
                        column: x => x.term,
                        principalTable: "words",
                        principalColumn: "term",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "translations");

            migrationBuilder.DropTable(
                name: "words");
        }
    }
}
