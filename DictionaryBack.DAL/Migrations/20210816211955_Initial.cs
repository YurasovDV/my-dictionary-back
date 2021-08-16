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
                    TermId = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    WordTerm = table.Column<string>(type: "character varying(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translations", x => new { x.TermId, x.meaning });
                    table.ForeignKey(
                        name: "FK_translations_words_WordTerm",
                        column: x => x.WordTerm,
                        principalTable: "words",
                        principalColumn: "term",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_translations_WordTerm",
                table: "translations",
                column: "WordTerm");
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
