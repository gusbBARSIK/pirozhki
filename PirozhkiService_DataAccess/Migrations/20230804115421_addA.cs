﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PirozhkiService_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationTypeId",
                table: "Product",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product",
                newName: "IX_Product_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationId",
                table: "Product",
                column: "ApplicationId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Product",
                newName: "ApplicationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ApplicationId",
                table: "Product",
                newName: "IX_Product_ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
