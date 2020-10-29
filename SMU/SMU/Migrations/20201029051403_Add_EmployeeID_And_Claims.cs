using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SMU.Migrations
{
    public partial class Add_EmployeeID_And_Claims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    ClaimDate = table.Column<DateTime>(nullable: false),
                    SentClaimDate = table.Column<DateTime>(nullable: false),
                    AttachmentPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "AspNetUsers");
        }
    }
}
