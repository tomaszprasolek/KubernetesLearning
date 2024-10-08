﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KubernetesTestApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "FirstName", "LastName", "Profession" },
                values: new object[] { 1, "John", "Doe Test", "programmer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
