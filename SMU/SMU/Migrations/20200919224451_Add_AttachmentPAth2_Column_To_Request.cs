using Microsoft.EntityFrameworkCore.Migrations;

namespace SMU.Migrations
{
    public partial class Add_AttachmentPAth2_Column_To_Request : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath2",
                table: "Requests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath2",
                table: "Requests");
        }
    }
}
