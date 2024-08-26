using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservice.Customer.Function.Migrations
{
    /// <inheritdoc />
    public partial class initialcreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MSOS_Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_Customer", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MSOS_Customer",
                columns: new[] { "Id", "Created", "Email", "FirstName", "LastUpdated", "Surname" },
                values: new object[,]
                {
                    { new Guid("2385de72-2302-4ced-866a-fa199116ca6e"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2784), "smateal@hotmail.com", "Sam", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2786), "Mateal" },
                    { new Guid("47417642-87d9-4047-ae13-4c721d99ab48"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2790), "tabertson@hotmail.com", "Tanya", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2791), "Abertson" },
                    { new Guid("55b431ff-693e-4664-8f65-cfd8d0b14b1b"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2779), "pmohammed@hotmail.com", "Mohammed", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2781), "Patel" },
                    { new Guid("5ff79dfe-c1fa-4dd9-996f-bc96649d6dfc"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2799), "borton@hotmail.com", "Beth", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2800), "Orton" },
                    { new Guid("aa1dc96f-3be5-41cd-8a1b-207284af3fdd"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2584), "jhopkins@hotmail.com", "Jane", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2755), "Hopkins" },
                    { new Guid("ae55b0d1-ba02-41e1-9efa-9b4d4ac15eec"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2806), "tamos@hotmail.com", "Tori", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2807), "Amos" },
                    { new Guid("af95fb7e-8d97-4892-8da3-5e6e51c54044"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2773), "jarthur@hotmail.com", "Arthur", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2774), "James" },
                    { new Guid("c95ba8ff-06a1-49d0-bc45-83f89b3ce820"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2810), "jpage@hotmail.com", "James", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2812), "Page" },
                    { new Guid("f07e88ac-53b2-4def-af07-957cbb18523c"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2818), "josbourne@hotmail.com", "John", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2820), "Osbourne" },
                    { new Guid("ff4d5a80-81e3-42e3-8052-92cf5c51e797"), new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2795), "stansor@hotmail.com", "Steven", new DateTime(2024, 3, 20, 19, 10, 56, 167, DateTimeKind.Local).AddTicks(2796), "Tansor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSOS_Customer");
        }
    }
}
