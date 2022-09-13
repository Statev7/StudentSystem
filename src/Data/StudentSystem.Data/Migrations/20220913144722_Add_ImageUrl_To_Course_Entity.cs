using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentSystem.Web.Data.Migrations
{
    public partial class Add_ImageUrl_To_Course_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Courses");
        }
    }
}
