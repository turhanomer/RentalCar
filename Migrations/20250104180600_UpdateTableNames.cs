using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCar.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiralamaTalepleri_Araclar_AracId",
                table: "KiralamaTalepleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yoneticiler",
                table: "Yoneticiler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KiralamaTalepleri",
                table: "KiralamaTalepleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Araclar",
                table: "Araclar");

            migrationBuilder.RenameTable(
                name: "Yoneticiler",
                newName: "Admins");

            migrationBuilder.RenameTable(
                name: "KiralamaTalepleri",
                newName: "RentalRequests");

            migrationBuilder.RenameTable(
                name: "Araclar",
                newName: "Cars");

            migrationBuilder.RenameColumn(
                name: "Sifre",
                table: "Admins",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "KullaniciAdi",
                table: "Admins",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "AdSoyad",
                table: "Admins",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "TalepTarihi",
                table: "RentalRequests",
                newName: "RequestDate");

            migrationBuilder.RenameColumn(
                name: "MusteriTelefon",
                table: "RentalRequests",
                newName: "CustomerPhone");

            migrationBuilder.RenameColumn(
                name: "MusteriEmail",
                table: "RentalRequests",
                newName: "CustomerEmail");

            migrationBuilder.RenameColumn(
                name: "MusteriAdi",
                table: "RentalRequests",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "RentalRequests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "BitisTarihi",
                table: "RentalRequests",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "BaslangicTarihi",
                table: "RentalRequests",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "AracId",
                table: "RentalRequests",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_KiralamaTalepleri_AracId",
                table: "RentalRequests",
                newName: "IX_RentalRequests_CarId");

            migrationBuilder.RenameColumn(
                name: "Yil",
                table: "Cars",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "ResimUrl",
                table: "Cars",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Musait",
                table: "Cars",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Marka",
                table: "Cars",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "GunlukFiyat",
                table: "Cars",
                newName: "DailyPrice");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Cars",
                newName: "Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalRequests",
                table: "RentalRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRequests_Cars_CarId",
                table: "RentalRequests",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRequests_Cars_CarId",
                table: "RentalRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalRequests",
                table: "RentalRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "RentalRequests",
                newName: "KiralamaTalepleri");

            migrationBuilder.RenameTable(
                name: "Cars",
                newName: "Araclar");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Yoneticiler");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "KiralamaTalepleri",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "KiralamaTalepleri",
                newName: "BaslangicTarihi");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "KiralamaTalepleri",
                newName: "TalepTarihi");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "KiralamaTalepleri",
                newName: "BitisTarihi");

            migrationBuilder.RenameColumn(
                name: "CustomerPhone",
                table: "KiralamaTalepleri",
                newName: "MusteriTelefon");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "KiralamaTalepleri",
                newName: "MusteriAdi");

            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "KiralamaTalepleri",
                newName: "MusteriEmail");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "KiralamaTalepleri",
                newName: "AracId");

            migrationBuilder.RenameIndex(
                name: "IX_RentalRequests_CarId",
                table: "KiralamaTalepleri",
                newName: "IX_KiralamaTalepleri_AracId");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Araclar",
                newName: "Yil");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Araclar",
                newName: "Musait");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Araclar",
                newName: "ResimUrl");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Araclar",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "DailyPrice",
                table: "Araclar",
                newName: "GunlukFiyat");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Araclar",
                newName: "Marka");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Yoneticiler",
                newName: "KullaniciAdi");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Yoneticiler",
                newName: "Sifre");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Yoneticiler",
                newName: "AdSoyad");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KiralamaTalepleri",
                table: "KiralamaTalepleri",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Araclar",
                table: "Araclar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yoneticiler",
                table: "Yoneticiler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KiralamaTalepleri_Araclar_AracId",
                table: "KiralamaTalepleri",
                column: "AracId",
                principalTable: "Araclar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
