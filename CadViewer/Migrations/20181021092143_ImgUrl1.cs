using Microsoft.EntityFrameworkCore.Migrations;

namespace CadViewer.Migrations
{
    public partial class ImgUrl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Material",
                newName: "imageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "Material",
                newName: "image");
        }
    }
}
