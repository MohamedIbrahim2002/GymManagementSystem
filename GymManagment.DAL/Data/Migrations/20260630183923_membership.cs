using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagment.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class membership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Plans_PlanId",
                table: "Memberships",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
