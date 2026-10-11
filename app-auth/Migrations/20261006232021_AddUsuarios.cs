using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_auth.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "Identificacion", "User", "PasswordHash", "Role" },
                values: new Object[,]
                {
                    { "1700000001", "Labcom", "AQAAAAIAAYagAAAAEEpoRWdkpuc3Db7w0sM1Wq/ib+66mhMp86B5hJCFVdhFyTPHF0pZVBYS925J/AQAEw==", "Cliente" },
                    { "1700000002", "Labcom1", "AQAAAAIAAYagAAAAEIaZbh1M/VqhXmzVeitbT0FLWGiZTVytdoJSr6hqZ+aiKnscrgLnWDDUCojCMS3xqA==", "Cliente" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
