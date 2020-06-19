using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Migrations
{
    public partial class updatedact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MinHrDay",
                table: "Activities",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MinHrDay",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
