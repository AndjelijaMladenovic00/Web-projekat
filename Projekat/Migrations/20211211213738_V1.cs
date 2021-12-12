using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hoteli",
                columns: table => new
                {
                    HotelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BrojSpratova = table.Column<int>(type: "int", nullable: false),
                    BrojSobaPoSpratu = table.Column<int>(type: "int", nullable: false),
                    Cena_I_kat = table.Column<int>(type: "int", nullable: false),
                    Cena_II_kat = table.Column<int>(type: "int", nullable: false),
                    Cena_III_kat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoteli", x => x.HotelID);
                });

            migrationBuilder.CreateTable(
                name: "Gosti",
                columns: table => new
                {
                    GostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrojLicneKarte = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gosti", x => x.GostID);
                    table.ForeignKey(
                        name: "FK_Gosti_Hoteli_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hoteli",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recepcioneri",
                columns: table => new
                {
                    RecepcionerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID_kartica = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepcioneri", x => x.RecepcionerID);
                    table.ForeignKey(
                        name: "FK_Recepcioneri_Hoteli_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hoteli",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sobe",
                columns: table => new
                {
                    SobaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojSobe = table.Column<int>(type: "int", nullable: false),
                    BrojKreveta = table.Column<int>(type: "int", nullable: false),
                    Kategorija = table.Column<int>(type: "int", nullable: false),
                    Izdata = table.Column<bool>(type: "bit", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: true),
                    RecepcionerID = table.Column<int>(type: "int", nullable: true),
                    GostID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sobe", x => x.SobaID);
                    table.ForeignKey(
                        name: "FK_Sobe_Gosti_GostID",
                        column: x => x.GostID,
                        principalTable: "Gosti",
                        principalColumn: "GostID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sobe_Hoteli_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hoteli",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sobe_Recepcioneri_RecepcionerID",
                        column: x => x.RecepcionerID,
                        principalTable: "Recepcioneri",
                        principalColumn: "RecepcionerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gosti_HotelID",
                table: "Gosti",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcioneri_HotelID",
                table: "Recepcioneri",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Sobe_GostID",
                table: "Sobe",
                column: "GostID");

            migrationBuilder.CreateIndex(
                name: "IX_Sobe_HotelID",
                table: "Sobe",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_Sobe_RecepcionerID",
                table: "Sobe",
                column: "RecepcionerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sobe");

            migrationBuilder.DropTable(
                name: "Gosti");

            migrationBuilder.DropTable(
                name: "Recepcioneri");

            migrationBuilder.DropTable(
                name: "Hoteli");
        }
    }
}
