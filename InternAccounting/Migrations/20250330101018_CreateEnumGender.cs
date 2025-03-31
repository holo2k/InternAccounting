using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternAccounting.Migrations
{
    /// <inheritdoc />
    public partial class CreateEnumGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:gender", "male,female,other");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:gender", "male,female,other");
        }
    }
}
