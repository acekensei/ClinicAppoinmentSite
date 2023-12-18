using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicAppointmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class SpellingMistake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TTimeSlot",
                table: "TimeSlots",
                newName: "Slot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slot",
                table: "TimeSlots",
                newName: "TTimeSlot");
        }
    }
}
