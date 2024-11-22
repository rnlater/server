using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationWithPublicationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameKnowledgeSubscriptions_Knowledges_KnowledgeId1",
                table: "GameKnowledgeSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_GameKnowledgeSubscriptions_KnowledgeId1",
                table: "GameKnowledgeSubscriptions");

            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("bf9fa3fd-e85f-4466-933b-326ff5ab53e7"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("3278d6d8-f3cf-4f6d-b059-847d2b4a32b4"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("39034003-e9d9-4f23-946e-441eb92886d7"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("49632a6c-4151-4766-8b8f-9193bfb9dc69"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("7b8b9ebd-a93b-4193-b3c2-ce1ecb6b7870"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("9a8bab58-1e73-44c6-ad67-0bbdd77dea4c"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("eecae7e3-d51c-43c0-9256-425d20dad231"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("c6a8a743-a025-4eee-ba7e-2571f023f62a") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("e8a35c41-da54-4148-81ec-a18da0797111") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("a4440e11-3bc7-425d-ac21-264b1906346a") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("f3d76cab-223d-49c3-b51b-e1ff2a57314c") });

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("a2e9c292-2b38-460f-99e5-62d27a48ebdd"));

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("ce793616-33aa-4b0b-9de9-447caef86a4c"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("d6da04a3-ead1-4083-9f03-395019d452c0"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("fda84451-1fca-429b-baea-401c798dea4a"));

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7") });

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae"), new Guid("1842dda3-63e2-419b-891c-844306444001") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7"), new Guid("d69d0960-ddf0-4f34-a636-bd8feeccc0b2") });

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("724e0ace-3973-49b2-962f-8dedb00ebd79"));

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("922c99ea-f068-4309-a359-ed3726321bef"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("c6a8a743-a025-4eee-ba7e-2571f023f62a"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("e8a35c41-da54-4148-81ec-a18da0797111"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("a4440e11-3bc7-425d-ac21-264b1906346a"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("f3d76cab-223d-49c3-b51b-e1ff2a57314c"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("120e4c04-fe9b-484a-b03a-189d83612813"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("dff36469-2a2c-4c7f-a462-4871e550b83d"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("1842dda3-63e2-419b-891c-844306444001"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("d69d0960-ddf0-4f34-a636-bd8feeccc0b2"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("6035bee5-cd3b-4608-a6a0-78eccfbb9ec9"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("c87995de-de81-4d48-ba8e-d574f55cc44b"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58"));

            migrationBuilder.DropColumn(
                name: "KnowledgeId1",
                table: "GameKnowledgeSubscriptions");

            migrationBuilder.CreateTable(
                name: "LearningLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LearnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningLists_Users_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PublicationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationRequests_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningListKnowledges",
                columns: table => new
                {
                    LearningListId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningListKnowledges", x => new { x.LearningListId, x.KnowledgeId });
                    table.ForeignKey(
                        name: "FK_LearningListKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningListKnowledges_LearningLists_LearningListId",
                        column: x => x.LearningListId,
                        principalTable: "LearningLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("1d6fa74e-c88f-4fcb-b7da-4f88f91c6f12"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7480), "Description 1", "image1.jpg", "Game 1" },
                    { new Guid("b7c255be-e122-458e-8664-7ce6f2383e76"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7490), "Description 2", "image2.jpg", "Game 2" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title" },
                values: new object[,]
                {
                    { new Guid("405a09dd-2964-440f-b1ad-058c1c222a6c"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7180), 1, null, "Algebra" },
                    { new Guid("a1e62b54-2a47-492b-98b4-5d72a8b722e3"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7180), 2, null, "Physics" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("b0ce8d13-338e-4579-9078-e390147a3ee4"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7130), "Theory", null },
                    { new Guid("f77de0fa-394f-4748-8441-a89d23b4fb4f"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7140), "Practical", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(6820), "Study of numbers, shapes, and patterns.", "Mathematics", "test.png" },
                    { new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(6830), "Study of the physical and natural world.", "Science", "test.png" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("49a7c9f6-0406-49b1-9190-da476d8a3a0f"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7090), "A track focused on Science.", "Science Track" },
                    { new Guid("925aa2a2-3407-48b5-9c2d-652b74cec34a"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7080), "A track focused on Mathematics.", "Mathematics Track" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UserName" },
                values: new object[] { new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(6110), "testuser@example.com", null, 1, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UserId" },
                values: new object[] { new Guid("17777d10-46a5-4d3c-a381-40055e563536"), null, null, new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(6450), "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", true, true, null, null, new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "Visibility" },
                values: new object[,]
                {
                    { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7230), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"), 0, "Introduction to Algebra", 0 },
                    { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7240), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"), 2, "Introduction to Physics", 0 }
                });

            migrationBuilder.InsertData(
                table: "LearningLists",
                columns: new[] { "Id", "CreatedAt", "LearnerId", "Title" },
                values: new object[,]
                {
                    { new Guid("88f3ab00-ff8f-4b90-b00c-107284079276"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7790), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"), "Learning List 2" },
                    { new Guid("e64cad03-3542-49eb-9c00-58a04f29940b"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7780), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"), "Learning List 1" }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224"), new Guid("49a7c9f6-0406-49b1-9190-da476d8a3a0f"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7360) },
                    { new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284"), new Guid("925aa2a2-3407-48b5-9c2d-652b74cec34a"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7360) }
                });

            migrationBuilder.InsertData(
                table: "GameKnowledgeSubscriptions",
                columns: new[] { "Id", "CreatedAt", "GameId", "KnowledgeId", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { new Guid("7e718c47-2fd2-4eec-88bd-db9c28ca7cb2"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7520), new Guid("1d6fa74e-c88f-4fcb-b7da-4f88f91c6f12"), new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), null, null },
                    { new Guid("c2f9c8a4-af73-48d9-909e-47d292220f81"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7530), new Guid("b7c255be-e122-458e-8664-7ce6f2383e76"), new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), null, null }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("405a09dd-2964-440f-b1ad-058c1c222a6c"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7450) },
                    { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("a1e62b54-2a47-492b-98b4-5d72a8b722e3"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7450) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("b0ce8d13-338e-4579-9078-e390147a3ee4"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7420) },
                    { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("f77de0fa-394f-4748-8441-a89d23b4fb4f"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7420) }
                });

            migrationBuilder.InsertData(
                table: "LearningListKnowledges",
                columns: new[] { "KnowledgeId", "LearningListId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("88f3ab00-ff8f-4b90-b00c-107284079276"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7820) },
                    { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("e64cad03-3542-49eb-9c00-58a04f29940b"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7820) }
                });

            migrationBuilder.InsertData(
                table: "Learnings",
                columns: new[] { "Id", "CreatedAt", "KnowledgeId", "ModifiedAt", "ModifiedBy", "NextReviewDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("522f4e6c-25ae-46b2-8db3-e9b7511fca2d"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7710), new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86") },
                    { new Guid("bbec6258-de5d-4932-a456-511da53fe382"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7710), new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86") }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type" },
                values: new object[,]
                {
                    { new Guid("01fccd72-2cb7-41a3-b500-4730b2c55756"), "Article about Physics.", new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7320), new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), 2, null, 2 },
                    { new Guid("90f4377d-d165-4e57-8e56-02573131c8af"), "Video content about Algebra.", new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7310), new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), 1, null, 5 }
                });

            migrationBuilder.InsertData(
                table: "PublicationRequests",
                columns: new[] { "Id", "CreatedAt", "KnowledgeId", "Status" },
                values: new object[,]
                {
                    { new Guid("96e508bf-7ded-4fe6-a772-b6bc717c9269"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7270), new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), 0 },
                    { new Guid("d0fbe966-749c-4f48-9632-517caadd3246"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7280), new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), 1 }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7390) },
                    { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7390) }
                });

            migrationBuilder.InsertData(
                table: "GameOptions",
                columns: new[] { "Id", "CreatedAt", "GameKnowledgeSubscriptionId", "Group", "IsCorrect", "Order", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("0b98309a-65eb-4d70-a597-d96f54c30993"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7560), new Guid("7e718c47-2fd2-4eec-88bd-db9c28ca7cb2"), 1, true, null, 1, "Value 1" },
                    { new Guid("412bfeb8-020f-438c-962c-2ce0734ace20"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7570), new Guid("7e718c47-2fd2-4eec-88bd-db9c28ca7cb2"), 1, null, null, 1, "Wrong Value" },
                    { new Guid("5119fa88-4ddf-4d5f-b58f-6d6afa9fdeed"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7570), new Guid("c2f9c8a4-af73-48d9-909e-47d292220f81"), 2, null, null, 0, "What is Value 2?" },
                    { new Guid("711b26ff-495b-4c55-8f4e-4e0c2ef8df92"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7580), new Guid("c2f9c8a4-af73-48d9-909e-47d292220f81"), 2, true, null, 1, "Value 2" },
                    { new Guid("d3085699-c0db-4453-aa90-a9f2969c426a"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7580), new Guid("c2f9c8a4-af73-48d9-909e-47d292220f81"), 2, null, null, 1, "Value 2" },
                    { new Guid("dd12808e-e97a-4254-bb93-a3f4dda5dca5"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7560), new Guid("7e718c47-2fd2-4eec-88bd-db9c28ca7cb2"), 1, null, null, 0, "What is Value 1?" }
                });

            migrationBuilder.InsertData(
                table: "LearningHistories",
                columns: new[] { "Id", "CreatedAt", "IsMemorized", "LearningId", "LearningLevel", "PlayedGameId", "Score" },
                values: new object[,]
                {
                    { new Guid("ad4df6a1-4f53-4129-8530-8718c414d13b"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7740), true, new Guid("522f4e6c-25ae-46b2-8db3-e9b7511fca2d"), 0, new Guid("1d6fa74e-c88f-4fcb-b7da-4f88f91c6f12"), 100 },
                    { new Guid("e3a724fa-4e89-4dde-afbd-fa7f227fd907"), new DateTime(2024, 11, 22, 11, 6, 55, 632, DateTimeKind.Utc).AddTicks(7750), false, new Guid("bbec6258-de5d-4932-a456-511da53fe382"), 1, new Guid("b7c255be-e122-458e-8664-7ce6f2383e76"), 80 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningListKnowledges_KnowledgeId",
                table: "LearningListKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningLists_LearnerId",
                table: "LearningLists",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationRequests_KnowledgeId",
                table: "PublicationRequests",
                column: "KnowledgeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningListKnowledges");

            migrationBuilder.DropTable(
                name: "PublicationRequests");

            migrationBuilder.DropTable(
                name: "LearningLists");

            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("17777d10-46a5-4d3c-a381-40055e563536"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("0b98309a-65eb-4d70-a597-d96f54c30993"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("412bfeb8-020f-438c-962c-2ce0734ace20"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("5119fa88-4ddf-4d5f-b58f-6d6afa9fdeed"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("711b26ff-495b-4c55-8f4e-4e0c2ef8df92"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("d3085699-c0db-4453-aa90-a9f2969c426a"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("dd12808e-e97a-4254-bb93-a3f4dda5dca5"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("405a09dd-2964-440f-b1ad-058c1c222a6c") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("a1e62b54-2a47-492b-98b4-5d72a8b722e3") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("b0ce8d13-338e-4579-9078-e390147a3ee4") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("f77de0fa-394f-4748-8441-a89d23b4fb4f") });

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("ad4df6a1-4f53-4129-8530-8718c414d13b"));

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("e3a724fa-4e89-4dde-afbd-fa7f227fd907"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("01fccd72-2cb7-41a3-b500-4730b2c55756"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("90f4377d-d165-4e57-8e56-02573131c8af"));

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"), new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284") });

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"), new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224"), new Guid("49a7c9f6-0406-49b1-9190-da476d8a3a0f") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284"), new Guid("925aa2a2-3407-48b5-9c2d-652b74cec34a") });

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("7e718c47-2fd2-4eec-88bd-db9c28ca7cb2"));

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("c2f9c8a4-af73-48d9-909e-47d292220f81"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("405a09dd-2964-440f-b1ad-058c1c222a6c"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("a1e62b54-2a47-492b-98b4-5d72a8b722e3"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("b0ce8d13-338e-4579-9078-e390147a3ee4"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("f77de0fa-394f-4748-8441-a89d23b4fb4f"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("522f4e6c-25ae-46b2-8db3-e9b7511fca2d"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("bbec6258-de5d-4932-a456-511da53fe382"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("927a88f4-114a-46fb-ae0d-ac23772c5284"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b4c4220e-c9fa-4fae-ae35-041e756d1224"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("49a7c9f6-0406-49b1-9190-da476d8a3a0f"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("925aa2a2-3407-48b5-9c2d-652b74cec34a"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("1d6fa74e-c88f-4fcb-b7da-4f88f91c6f12"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("b7c255be-e122-458e-8664-7ce6f2383e76"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("5a5fb990-9636-4ed8-ae0b-52a978a821ef"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("8c57b1be-9f45-424c-8a38-6a99438f07cd"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96d38ccc-cbae-4e68-bb50-f423dee7fb86"));

            migrationBuilder.AddColumn<Guid>(
                name: "KnowledgeId1",
                table: "GameKnowledgeSubscriptions",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("6035bee5-cd3b-4608-a6a0-78eccfbb9ec9"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5360), "Description 1", "image1.jpg", "Game 1" },
                    { new Guid("c87995de-de81-4d48-ba8e-d574f55cc44b"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5370), "Description 2", "image2.jpg", "Game 2" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title" },
                values: new object[,]
                {
                    { new Guid("c6a8a743-a025-4eee-ba7e-2571f023f62a"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5050), 1, null, "Algebra" },
                    { new Guid("e8a35c41-da54-4148-81ec-a18da0797111"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5130), 2, null, "Physics" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("a4440e11-3bc7-425d-ac21-264b1906346a"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5010), "Practical", null },
                    { new Guid("f3d76cab-223d-49c3-b51b-e1ff2a57314c"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5010), "Theory", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4920), "Study of numbers, shapes, and patterns.", "Mathematics", "test.png" },
                    { new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4930), "Study of the physical and natural world.", "Science", "test.png" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("1842dda3-63e2-419b-891c-844306444001"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4970), "A track focused on Science.", "Science Track" },
                    { new Guid("d69d0960-ddf0-4f34-a636-bd8feeccc0b2"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4960), "A track focused on Mathematics.", "Mathematics Track" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UserName" },
                values: new object[] { new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4330), "testuser@example.com", null, 1, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UserId" },
                values: new object[] { new Guid("bf9fa3fd-e85f-4466-933b-326ff5ab53e7"), null, null, new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(4560), "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", true, true, null, null, new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "Visibility" },
                values: new object[,]
                {
                    { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5170), new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58"), 0, "Introduction to Algebra", 0 },
                    { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5180), new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58"), 2, "Introduction to Physics", 0 }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae"), new Guid("1842dda3-63e2-419b-891c-844306444001"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5250) },
                    { new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7"), new Guid("d69d0960-ddf0-4f34-a636-bd8feeccc0b2"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5250) }
                });

            migrationBuilder.InsertData(
                table: "GameKnowledgeSubscriptions",
                columns: new[] { "Id", "CreatedAt", "GameId", "KnowledgeId", "KnowledgeId1", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { new Guid("724e0ace-3973-49b2-962f-8dedb00ebd79"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5400), new Guid("c87995de-de81-4d48-ba8e-d574f55cc44b"), new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), null, null, null },
                    { new Guid("922c99ea-f068-4309-a359-ed3726321bef"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5400), new Guid("6035bee5-cd3b-4608-a6a0-78eccfbb9ec9"), new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), null, null, null }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("c6a8a743-a025-4eee-ba7e-2571f023f62a"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5330) },
                    { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("e8a35c41-da54-4148-81ec-a18da0797111"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5330) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("a4440e11-3bc7-425d-ac21-264b1906346a"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5300) },
                    { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("f3d76cab-223d-49c3-b51b-e1ff2a57314c"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5300) }
                });

            migrationBuilder.InsertData(
                table: "Learnings",
                columns: new[] { "Id", "CreatedAt", "KnowledgeId", "ModifiedAt", "ModifiedBy", "NextReviewDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("120e4c04-fe9b-484a-b03a-189d83612813"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5600), new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58") },
                    { new Guid("dff36469-2a2c-4c7f-a462-4871e550b83d"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5610), new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a9b57b10-671f-49bd-86a9-ac07ba03fc58") }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type" },
                values: new object[,]
                {
                    { new Guid("d6da04a3-ead1-4083-9f03-395019d452c0"), "Article about Physics.", new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5220), new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), 2, null, 2 },
                    { new Guid("fda84451-1fca-429b-baea-401c798dea4a"), "Video content about Algebra.", new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5210), new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), 1, null, 5 }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("5b9fe9f4-8553-4f91-a806-8e19a2dae9b6"), new Guid("e2076035-11cb-473e-8c0c-9c68bc711da7"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5280) },
                    { new Guid("6b7012bd-6663-428a-b187-73b15ac9e8cc"), new Guid("e9e4cdbe-ae71-4def-9f21-3dc8bb5035ae"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5280) }
                });

            migrationBuilder.InsertData(
                table: "GameOptions",
                columns: new[] { "Id", "CreatedAt", "GameKnowledgeSubscriptionId", "Group", "IsCorrect", "Order", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("3278d6d8-f3cf-4f6d-b059-847d2b4a32b4"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5440), new Guid("922c99ea-f068-4309-a359-ed3726321bef"), 1, true, null, 1, "Value 1" },
                    { new Guid("39034003-e9d9-4f23-946e-441eb92886d7"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5560), new Guid("724e0ace-3973-49b2-962f-8dedb00ebd79"), 2, true, null, 1, "Value 2" },
                    { new Guid("49632a6c-4151-4766-8b8f-9193bfb9dc69"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5440), new Guid("922c99ea-f068-4309-a359-ed3726321bef"), 1, null, null, 1, "Wrong Value" },
                    { new Guid("7b8b9ebd-a93b-4193-b3c2-ce1ecb6b7870"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5430), new Guid("922c99ea-f068-4309-a359-ed3726321bef"), 1, null, null, 0, "What is Value 1?" },
                    { new Guid("9a8bab58-1e73-44c6-ad67-0bbdd77dea4c"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5560), new Guid("724e0ace-3973-49b2-962f-8dedb00ebd79"), 2, null, null, 1, "Value 2" },
                    { new Guid("eecae7e3-d51c-43c0-9256-425d20dad231"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5450), new Guid("724e0ace-3973-49b2-962f-8dedb00ebd79"), 2, null, null, 0, "What is Value 2?" }
                });

            migrationBuilder.InsertData(
                table: "LearningHistories",
                columns: new[] { "Id", "CreatedAt", "IsMemorized", "LearningId", "LearningLevel", "PlayedGameId", "Score" },
                values: new object[,]
                {
                    { new Guid("a2e9c292-2b38-460f-99e5-62d27a48ebdd"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5640), false, new Guid("dff36469-2a2c-4c7f-a462-4871e550b83d"), 1, new Guid("c87995de-de81-4d48-ba8e-d574f55cc44b"), 80 },
                    { new Guid("ce793616-33aa-4b0b-9de9-447caef86a4c"), new DateTime(2024, 11, 15, 15, 44, 30, 583, DateTimeKind.Utc).AddTicks(5640), true, new Guid("120e4c04-fe9b-484a-b03a-189d83612813"), 0, new Guid("6035bee5-cd3b-4608-a6a0-78eccfbb9ec9"), 100 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameKnowledgeSubscriptions_KnowledgeId1",
                table: "GameKnowledgeSubscriptions",
                column: "KnowledgeId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameKnowledgeSubscriptions_Knowledges_KnowledgeId1",
                table: "GameKnowledgeSubscriptions",
                column: "KnowledgeId1",
                principalTable: "Knowledges",
                principalColumn: "Id");
        }
    }
}
