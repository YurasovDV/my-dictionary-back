using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DictionaryBack.DAL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 200, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    term = table.Column<string>(type: "citext", nullable: false),
                    topic_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_repetition = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    repetition_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.term);
                    table.ForeignKey(
                        name: "FK_words_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    meaning = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    term = table.Column<string>(type: "citext", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_topics_name",
                table: "topics",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_words_topic_id",
                table: "words",
                column: "topic_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "translations");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "topics");
        }
    }
}
