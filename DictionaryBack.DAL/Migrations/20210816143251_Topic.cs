using Microsoft.EntityFrameworkCore.Migrations;

namespace DictionaryBack.DAL.Migrations
{
    public partial class Topic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Words",
                type: "text",
                nullable: true,
                defaultValue: "user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Words");
        }
    }
}
