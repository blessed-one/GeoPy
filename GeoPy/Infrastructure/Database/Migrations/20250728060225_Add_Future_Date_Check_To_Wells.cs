using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Future_Date_Check_To_Wells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "ck_well_measurement_date_not_future",
                table: "wells",
                sql: "\"measurement_date\" <= CURRENT_DATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_well_measurement_date_not_future",
                table: "wells");
        }
    }
}
