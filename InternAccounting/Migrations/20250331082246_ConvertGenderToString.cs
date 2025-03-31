using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternAccounting.Migrations
{
    /// <inheritdoc />
    public partial class ConvertGenderToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:gender", "male,female,other");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Interns",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "gender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:gender", "male,female,other");

            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "Interns",
                type: "gender",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)");
        }
    }
}
