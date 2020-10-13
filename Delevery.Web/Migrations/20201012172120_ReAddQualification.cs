using Microsoft.EntityFrameworkCore.Migrations;

namespace Delevery.Web.Migrations
{
    public partial class ReAddQualification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qualifications_Restaurants_ProductId",
                table: "Qualifications");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Qualifications",
                newName: "RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Qualifications_ProductId",
                table: "Qualifications",
                newName: "IX_Qualifications_RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Qualifications_Restaurants_RestaurantId",
                table: "Qualifications",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qualifications_Restaurants_RestaurantId",
                table: "Qualifications");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Qualifications",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Qualifications_RestaurantId",
                table: "Qualifications",
                newName: "IX_Qualifications_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Qualifications_Restaurants_ProductId",
                table: "Qualifications",
                column: "ProductId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
