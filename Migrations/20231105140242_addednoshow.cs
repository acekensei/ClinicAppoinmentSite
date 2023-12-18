using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicAppointmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class addednoshow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NoShow",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoShow",
                table: "Appointments");
        }
    }
}
