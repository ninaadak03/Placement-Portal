using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Changes3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OtpVerifications_Email",
                table: "OtpVerifications");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerifications_Email",
                table: "OtpVerifications",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OtpVerifications_Email",
                table: "OtpVerifications");

            migrationBuilder.CreateIndex(
                name: "IX_OtpVerifications_Email",
                table: "OtpVerifications",
                column: "Email");
        }
    }
}
