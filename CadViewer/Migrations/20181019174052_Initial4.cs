﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CadViewer.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "image",
                table: "Material",
                nullable: true,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "image",
                table: "Material",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
