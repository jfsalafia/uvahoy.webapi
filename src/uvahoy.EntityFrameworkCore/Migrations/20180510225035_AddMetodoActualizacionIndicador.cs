using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace uvahoy.Migrations
{
    public partial class AddMetodoActualizacionIndicador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormatoDatos",
                table: "Indicador",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoActualizacion",
                table: "Indicador",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatoDatos",
                table: "Indicador");

            migrationBuilder.DropColumn(
                name: "MetodoActualizacion",
                table: "Indicador");
        }
    }
}
