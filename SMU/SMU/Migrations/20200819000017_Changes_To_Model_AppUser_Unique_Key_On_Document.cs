using Microsoft.EntityFrameworkCore.Migrations;

namespace SMU.Migrations
{
    public partial class Changes_To_Model_AppUser_Unique_Key_On_Document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSupervisor",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Supervisor",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Document",
                table: "AspNetUsers",
                column: "Document",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Document",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Supervisor",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "IdSupervisor",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }
    }
}
