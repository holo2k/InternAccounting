using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternAccounting.Migrations
{
    /// <inheritdoc />
    public partial class FixGenderColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "Interns",
                type: "gender",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "Interns",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "gender");
        }
    }
}
