using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BENT1C.Grupo4.Migrations
{
    public partial class PrimeraMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 75, nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Miembro",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 75, nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true),
                    Telefono = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miembro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrada",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Privada = table.Column<bool>(nullable: false),
                    CategoriaId = table.Column<Guid>(nullable: false),
                    MiembroId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrada_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntradaMiembro",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdEntrada = table.Column<Guid>(nullable: false),
                    IdMiembro = table.Column<Guid>(nullable: false),
                    Habilitado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaMiembro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradaMiembro_Entrada_IdEntrada",
                        column: x => x.IdEntrada,
                        principalTable: "Entrada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntradaMiembro_Miembro_IdMiembro",
                        column: x => x.IdMiembro,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregunta",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Activa = table.Column<bool>(nullable: false),
                    EntradaId = table.Column<Guid>(nullable: false),
                    MiembroId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregunta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pregunta_Entrada_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entrada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pregunta_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respuesta",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    PreguntaId = table.Column<Guid>(nullable: false),
                    MiembroId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuesta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respuesta_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Respuesta_Pregunta_PreguntaId",
                        column: x => x.PreguntaId,
                        principalTable: "Pregunta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    MeGusta = table.Column<bool>(nullable: false),
                    RespuestaId = table.Column<Guid>(nullable: false),
                    MiembroId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Like_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Like_Respuesta_RespuestaId",
                        column: x => x.RespuestaId,
                        principalTable: "Respuesta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_CategoriaId",
                table: "Entrada",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_MiembroId",
                table: "Entrada",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaMiembro_IdEntrada",
                table: "EntradaMiembro",
                column: "IdEntrada");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaMiembro_IdMiembro",
                table: "EntradaMiembro",
                column: "IdMiembro");

            migrationBuilder.CreateIndex(
                name: "IX_Like_MiembroId",
                table: "Like",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_RespuestaId",
                table: "Like",
                column: "RespuestaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_EntradaId",
                table: "Pregunta",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_MiembroId",
                table: "Pregunta",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuesta_MiembroId",
                table: "Respuesta",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuesta_PreguntaId",
                table: "Respuesta",
                column: "PreguntaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "EntradaMiembro");

            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "Respuesta");

            migrationBuilder.DropTable(
                name: "Pregunta");

            migrationBuilder.DropTable(
                name: "Entrada");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Miembro");
        }
    }
}
