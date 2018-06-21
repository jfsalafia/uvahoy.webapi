using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace uvahoy.Migrations
{
    public partial class AddIndicadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indicador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abreviatura = table.Column<string>(nullable: true),
                    Activo = table.Column<bool>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 2048, nullable: true),
                    FuenteDatos = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cotizacion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FechaHoraCotizacion = table.Column<DateTime>(nullable: false),
                    IndicadorId = table.Column<int>(nullable: false),
                    ValorCotizacion = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cotizacion_Indicador_IndicadorId",
                        column: x => x.IndicadorId,
                        principalTable: "Indicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicadorUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    IndicadorId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadorUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicadorUsuario_Indicador_IndicadorId",
                        column: x => x.IndicadorId,
                        principalTable: "Indicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndicadorUsuario_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_IndicadorId",
                table: "Cotizacion",
                column: "IndicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorUsuario_IndicadorId",
                table: "IndicadorUsuario",
                column: "IndicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorUsuario_UserId",
                table: "IndicadorUsuario",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cotizacion");

            migrationBuilder.DropTable(
                name: "IndicadorUsuario");

            migrationBuilder.DropTable(
                name: "Indicador");
        }
    }
}
