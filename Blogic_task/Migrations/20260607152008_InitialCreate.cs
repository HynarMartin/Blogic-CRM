using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogic_task.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klienti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jmeno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prijmeni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RodneCislo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumNarozeni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klienti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poradci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jmeno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prijmeni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RodneCislo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumNarozeni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poradci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Smlouvy",
                columns: table => new
                {
                    EvidencniCislo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Instituce = table.Column<int>(type: "int", nullable: false),
                    DatumUzavreni = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPlatnosti = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumUkonceni = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KlientId = table.Column<int>(type: "int", nullable: false),
                    SpravceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smlouvy", x => x.EvidencniCislo);
                    table.ForeignKey(
                        name: "FK_Smlouvy_Klienti_KlientId",
                        column: x => x.KlientId,
                        principalTable: "Klienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Smlouvy_Poradci_SpravceId",
                        column: x => x.SpravceId,
                        principalTable: "Poradci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmlouvaDalsiPoradce",
                columns: table => new
                {
                    DalsiPoradciId = table.Column<int>(type: "int", nullable: false),
                    DalsiSmlouvyEvidencniCislo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmlouvaDalsiPoradce", x => new { x.DalsiPoradciId, x.DalsiSmlouvyEvidencniCislo });
                    table.ForeignKey(
                        name: "FK_SmlouvaDalsiPoradce_Poradci_DalsiPoradciId",
                        column: x => x.DalsiPoradciId,
                        principalTable: "Poradci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmlouvaDalsiPoradce_Smlouvy_DalsiSmlouvyEvidencniCislo",
                        column: x => x.DalsiSmlouvyEvidencniCislo,
                        principalTable: "Smlouvy",
                        principalColumn: "EvidencniCislo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmlouvaDalsiPoradce_DalsiSmlouvyEvidencniCislo",
                table: "SmlouvaDalsiPoradce",
                column: "DalsiSmlouvyEvidencniCislo");

            migrationBuilder.CreateIndex(
                name: "IX_Smlouvy_KlientId",
                table: "Smlouvy",
                column: "KlientId");

            migrationBuilder.CreateIndex(
                name: "IX_Smlouvy_SpravceId",
                table: "Smlouvy",
                column: "SpravceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmlouvaDalsiPoradce");

            migrationBuilder.DropTable(
                name: "Smlouvy");

            migrationBuilder.DropTable(
                name: "Klienti");

            migrationBuilder.DropTable(
                name: "Poradci");
        }
    }
}
