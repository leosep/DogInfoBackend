using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogInfo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LifeSpanMin = table.Column<int>(type: "int", nullable: false),
                    LifeSpanMax = table.Column<int>(type: "int", nullable: false),
                    MaleWeightMin = table.Column<int>(type: "int", nullable: false),
                    MaleWeightMax = table.Column<int>(type: "int", nullable: false),
                    FemaleWeightMin = table.Column<int>(type: "int", nullable: false),
                    FemaleWeightMax = table.Column<int>(type: "int", nullable: false),
                    Hypoallergenic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breeds");
        }
    }
}
