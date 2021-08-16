using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.Dotnet.DAL.Migrations
{
    public partial class RemoveCommentParrentForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentID",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentID",
                table: "Comments",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentID",
                table: "Comments",
                column: "ParentID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
