using Microsoft.EntityFrameworkCore.Migrations;

namespace SMU.Migrations
{
    public partial class TableChanges_Document_To_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Documento",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documento",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "AspNetUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
