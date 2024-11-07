using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAssessment.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnumDesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayDesc",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatusDesc",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentGatewayDesc",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentStatusDesc",
                table: "Orders");
        }
    }
}
