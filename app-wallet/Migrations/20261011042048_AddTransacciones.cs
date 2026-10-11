using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_wallet.Migrations
{
    /// <inheritdoc />
    public partial class AddTransacciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "cuentas",
                columns: new[] { "NumeroCuenta", "Saldo" },
                values: new object[,]
                {
                    { "1700000001", 500.00m },
                    { "1700000002", 150.50m }
                });

            migrationBuilder.InsertData(
                table: "transacciones",
                columns: new[] { "Id", "Fecha", "Tipo", "Valor", "SaldoDisponible", "NumeroCuenta" },
                values: new object[,]
                {
                    { 1, DateTime.UtcNow, "Credito", 500.00m, 500.00m, "1700000001" },
                    { 2, DateTime.UtcNow.AddDays(-2), "Credito", 200.00m, 200.00m, "1700000002" },
                    { 3, DateTime.UtcNow.AddDays(-1), "Debito", 49.50m, 150.50m, "1700000002" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Borramos las transacciones
            migrationBuilder.DeleteData(
                table: "transacciones",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });

            // 2. Borramos las cuentas
            migrationBuilder.DeleteData(
                table: "cuentas",
                keyColumn: "NumeroCuenta",
                keyValues: new object[] { "1700000001", "1700000002" });
        }
    }
}
