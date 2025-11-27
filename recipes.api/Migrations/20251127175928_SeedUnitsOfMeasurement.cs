using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recipes.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedUnitsOfMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitsOfMeasurement",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00a0409c-8239-4118-9938-8feb598cfe31"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4956), "gram", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4957) },
                    { new Guid("1289a211-b2a9-4d05-89f6-b30979271b16"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4986), "bottle", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4986) },
                    { new Guid("169ac0d3-a029-471b-a285-87f04f6c80ca"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4962), "liter", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4962) },
                    { new Guid("33240855-a90f-4eae-aed9-e5505a760ec7"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4974), "slice", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4975) },
                    { new Guid("4aed97c5-f741-4a2e-bc8c-bccb6e1d5998"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4967), "pinch", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4967) },
                    { new Guid("4fea1e82-8f1d-4b03-95f1-dd167c18c41c"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4984), "jar", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4984) },
                    { new Guid("537debe5-cdfc-482e-8fb2-6e0956524976"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4952), "ounce", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4953) },
                    { new Guid("9a0aa1c9-658b-4769-abd2-7280f6cb9e38"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4960), "milliliter", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4960) },
                    { new Guid("b000f375-1dae-4200-a0bf-d48614dc7b0d"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4976), "clove", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4977) },
                    { new Guid("b5bdab3f-077d-442b-9b4e-e4aa0761e7d4"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4980), "package", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4980) },
                    { new Guid("cec58f72-bbb8-473e-98ac-a8b153c66f77"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4928), "cup", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4934) },
                    { new Guid("d38a828c-79d9-44f7-a912-d912b97cd1ec"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4954), "pound", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4955) },
                    { new Guid("dc21eba3-b1f6-4575-8050-739290e6cb4e"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4978), "can", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4978) },
                    { new Guid("e507aecc-50a0-45d7-b43a-77bb9b3b701c"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4948), "tablespoon", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4949) },
                    { new Guid("e93ac73d-dc29-43ba-b838-2bc23f521e3b"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4970), "whole", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4971) },
                    { new Guid("eabd7030-1f6d-493f-92b8-1e0b4a25b67f"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4972), "piece", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4973) },
                    { new Guid("ece62990-f684-4ab9-843f-4d2d076fedf5"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4987), "box", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4988) },
                    { new Guid("eeed0d4f-c3df-4665-8712-ba392490dce6"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4958), "kilogram", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4958) },
                    { new Guid("efc6654e-9a41-405b-8e2a-14f8e543c3c3"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4969), "dash", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4969) },
                    { new Guid("fecd3df0-1165-4c99-9ea3-589867ae8e19"), new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4951), "teaspoon", new DateTime(2025, 11, 27, 17, 59, 28, 512, DateTimeKind.Utc).AddTicks(4951) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("00a0409c-8239-4118-9938-8feb598cfe31"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("1289a211-b2a9-4d05-89f6-b30979271b16"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("169ac0d3-a029-471b-a285-87f04f6c80ca"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("33240855-a90f-4eae-aed9-e5505a760ec7"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("4aed97c5-f741-4a2e-bc8c-bccb6e1d5998"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("4fea1e82-8f1d-4b03-95f1-dd167c18c41c"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("537debe5-cdfc-482e-8fb2-6e0956524976"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("9a0aa1c9-658b-4769-abd2-7280f6cb9e38"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("b000f375-1dae-4200-a0bf-d48614dc7b0d"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("b5bdab3f-077d-442b-9b4e-e4aa0761e7d4"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("cec58f72-bbb8-473e-98ac-a8b153c66f77"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("d38a828c-79d9-44f7-a912-d912b97cd1ec"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("dc21eba3-b1f6-4575-8050-739290e6cb4e"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("e507aecc-50a0-45d7-b43a-77bb9b3b701c"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("e93ac73d-dc29-43ba-b838-2bc23f521e3b"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("eabd7030-1f6d-493f-92b8-1e0b4a25b67f"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("ece62990-f684-4ab9-843f-4d2d076fedf5"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("eeed0d4f-c3df-4665-8712-ba392490dce6"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("efc6654e-9a41-405b-8e2a-14f8e543c3c3"));

            migrationBuilder.DeleteData(
                table: "UnitsOfMeasurement",
                keyColumn: "Id",
                keyValue: new Guid("fecd3df0-1165-4c99-9ea3-589867ae8e19"));
        }
    }
}
