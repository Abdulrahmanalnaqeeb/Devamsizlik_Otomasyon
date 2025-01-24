using Microsoft.EntityFrameworkCore.Migrations;

namespace DevamsizlikTakip.Migrations
{
    public partial class UpdateDatabase_2024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Devamsizlik tablosunu güncelle
            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Devamsizliklar",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // User tablosunu güncelle
            migrationBuilder.AlterColumn<string>(
                name: "Sinif",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Departman",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemleri
            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Devamsizliklar",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Sinif",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Departman",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false);
        }
    }
} 