using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PivotTableModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("2f687634-5913-4f18-a465-acf5756feb52"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("4fbdda70-cb0f-491c-a03f-b1ad06de36e0") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("f53e7372-de53-481b-88ec-48146b669d74") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("07ee7c83-7b14-4086-af9a-188c831add83") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("de1e4f7b-02ab-45a8-ba17-4ee75119b461") });

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("5bd6e02d-03e4-4efd-8104-d1b00aaeee88"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("869b100e-1604-429f-a441-4ec4f8a93086"));

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc") });

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc"), new Guid("1c537db9-6b69-4134-89d2-6c3ca50a2fb0") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305"), new Guid("83b8c609-11bc-45d3-8d95-c566b643797e") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("4fbdda70-cb0f-491c-a03f-b1ad06de36e0"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("f53e7372-de53-481b-88ec-48146b669d74"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("07ee7c83-7b14-4086-af9a-188c831add83"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("de1e4f7b-02ab-45a8-ba17-4ee75119b461"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("1c537db9-6b69-4134-89d2-6c3ca50a2fb0"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("83b8c609-11bc-45d3-8d95-c566b643797e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("242ea95f-e14a-4b52-a134-302aa68daa6a"));

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "KnowledgeTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "KnowledgeTopics");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Knowledges");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Authentications");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TrackSubjects",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "SubjectKnowledges",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "KnowledgeTypeKnowledges",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "KnowledgeTopicKnowledges",
                newName: "CreatedAt");

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title" },
                values: new object[,]
                {
                    { new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8090), 1, null, "Algebra" },
                    { new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8100), 2, null, "Physics" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8060), "Practical", null },
                    { new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8050), "Theory", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7980), "Study of the physical and natural world.", "Science", "test.png" },
                    { new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7970), "Study of numbers, shapes, and patterns.", "Mathematics", "test.png" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8020), "A track focused on Science.", "Science Track" },
                    { new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8010), "A track focused on Mathematics.", "Mathematics Track" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UserName" },
                values: new object[] { new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7300), "testuser@example.com", null, 1, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UserId" },
                values: new object[] { new Guid("45007bfc-f89a-4c89-8534-8fef4b26c41f"), null, null, new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7600), "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", true, true, null, null, new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "Visibility" },
                values: new object[,]
                {
                    { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8220), new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"), 0, "Introduction to Physics", 0 },
                    { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8210), new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"), 0, "Introduction to Algebra", 0 }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"), new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8300) },
                    { new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"), new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8300) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8370) },
                    { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8370) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8350) },
                    { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8350) }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type" },
                values: new object[,]
                {
                    { new Guid("53f2fa8f-29df-4adc-b323-3fb61f70293f"), "Article about Physics.", new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8260), new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), 2, null, 0 },
                    { new Guid("b1c8a2be-195e-4358-bc52-3a52bea54315"), "Video content about Algebra.", new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8250), new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), 1, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8330) },
                    { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"), new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8320) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("45007bfc-f89a-4c89-8534-8fef4b26c41f"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e") });

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("53f2fa8f-29df-4adc-b323-3fb61f70293f"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("b1c8a2be-195e-4358-bc52-3a52bea54315"));

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"), new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8") });

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"), new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"), new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"), new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"));

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TrackSubjects",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SubjectKnowledges",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "KnowledgeTypeKnowledges",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "KnowledgeTopicKnowledges",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tracks",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Subjects",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Materials",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "KnowledgeTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "KnowledgeTopics",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Knowledges",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Authentications",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4fbdda70-cb0f-491c-a03f-b1ad06de36e0"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7480), 1, null, "Algebra", null },
                    { new Guid("f53e7372-de53-481b-88ec-48146b669d74"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7480), 2, null, "Physics", null }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("07ee7c83-7b14-4086-af9a-188c831add83"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7440), "Practical", null, null },
                    { new Guid("de1e4f7b-02ab-45a8-ba17-4ee75119b461"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7440), "Theory", null, null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Photo", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7370), "Study of the physical and natural world.", "Science", "test.png", null },
                    { new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7370), "Study of numbers, shapes, and patterns.", "Mathematics", "test.png", null }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1c537db9-6b69-4134-89d2-6c3ca50a2fb0"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7410), "A track focused on Science.", "Science Track", null },
                    { new Guid("83b8c609-11bc-45d3-8d95-c566b643797e"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7400), "A track focused on Mathematics.", "Mathematics Track", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("242ea95f-e14a-4b52-a134-302aa68daa6a"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(6750), "testuser@example.com", null, 1, null, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("2f687634-5913-4f18-a465-acf5756feb52"), null, null, new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(6970), "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", true, true, null, null, null, new Guid("242ea95f-e14a-4b52-a134-302aa68daa6a") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "UpdatedAt", "Visibility" },
                values: new object[,]
                {
                    { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7580), new Guid("242ea95f-e14a-4b52-a134-302aa68daa6a"), 0, "Introduction to Algebra", null, 0 },
                    { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7590), new Guid("242ea95f-e14a-4b52-a134-302aa68daa6a"), 0, "Introduction to Physics", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId", "CreatedDate" },
                values: new object[,]
                {
                    { new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc"), new Guid("1c537db9-6b69-4134-89d2-6c3ca50a2fb0"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7670) },
                    { new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305"), new Guid("83b8c609-11bc-45d3-8d95-c566b643797e"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7660) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId", "CreatedDate" },
                values: new object[,]
                {
                    { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("4fbdda70-cb0f-491c-a03f-b1ad06de36e0"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7740) },
                    { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("f53e7372-de53-481b-88ec-48146b669d74"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7750) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId", "CreatedDate" },
                values: new object[,]
                {
                    { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("07ee7c83-7b14-4086-af9a-188c831add83"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7720) },
                    { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("de1e4f7b-02ab-45a8-ba17-4ee75119b461"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7720) }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("5bd6e02d-03e4-4efd-8104-d1b00aaeee88"), "Video content about Algebra.", new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7620), new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), 1, null, 2, null },
                    { new Guid("869b100e-1604-429f-a441-4ec4f8a93086"), "Article about Physics.", new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7630), new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), 2, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId", "CreatedDate" },
                values: new object[,]
                {
                    { new Guid("e605907f-5fd3-42e2-82d5-b017a578a72c"), new Guid("14524d0d-3ac4-4669-ae39-8c7ec2affdbc"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7690) },
                    { new Guid("89ab59a8-d916-418b-bf38-7a9e9236dc96"), new Guid("6524c09d-bb1c-4586-a2b3-b9d4767d8305"), new DateTime(2024, 11, 4, 8, 27, 14, 236, DateTimeKind.Utc).AddTicks(7690) }
                });
        }
    }
}
