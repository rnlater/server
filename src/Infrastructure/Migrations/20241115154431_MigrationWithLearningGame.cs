using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationWithLearningGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("73634806-5e73-411c-afcc-361bd3fd972e"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("00b4f54f-ea0b-4a42-8b35-562ab18fc60f"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("38465583-45fb-4e13-bc31-261fbee57e75"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("410a3ef3-91d3-4790-94c7-2c34599cc36b"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("77ab178b-132d-4b58-b46f-8fada61cd3c0"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("802cd8b9-4e45-47b1-a462-6f21d7cb69ee"));

            migrationBuilder.DeleteData(
                table: "GameOptions",
                keyColumn: "Id",
                keyValue: new Guid("bfa7c1d4-b42b-4cce-9729-8b1b7f315bb6"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("5e7aa25a-3b50-449a-a9da-443d8174e1aa") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTopicKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                keyValues: new object[] { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("9c4c2c9c-4c2e-433f-8cb5-2ba74dcdf7b7") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("1ef91520-f995-4871-8d61-11386ae17d5f") });

            migrationBuilder.DeleteData(
                table: "KnowledgeTypeKnowledges",
                keyColumns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                keyValues: new object[] { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("ae87764f-e6c7-44d5-ad32-2fb91fc8ce32") });

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("1737db4c-21f0-44d3-bdbc-9b2c4541a310"));

            migrationBuilder.DeleteData(
                table: "LearningHistories",
                keyColumn: "Id",
                keyValue: new Guid("e8983958-c302-4907-b6ba-4cd3d6428b1d"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("b5ce233f-83a6-4424-8301-81f852ae0b69"));

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: new Guid("d6d7f7c7-a66f-4281-942d-a1c8793449f0"));

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e") });

            migrationBuilder.DeleteData(
                table: "SubjectKnowledges",
                keyColumns: new[] { "KnowledgeId", "SubjectId" },
                keyValues: new object[] { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e"), new Guid("4b8864c1-8755-4d73-8b7b-00cdce57a74a") });

            migrationBuilder.DeleteData(
                table: "TrackSubjects",
                keyColumns: new[] { "SubjectId", "TrackId" },
                keyValues: new object[] { new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e"), new Guid("a2c7d934-b843-49b8-9d0f-3daa10c98697") });

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("1dcf2b12-8b2c-4d8e-b992-261fdf07e5c9"));

            migrationBuilder.DeleteData(
                table: "GameKnowledgeSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("b7811e7a-6df6-4d7d-a81f-f873168f0f9b"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("5e7aa25a-3b50-449a-a9da-443d8174e1aa"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTopics",
                keyColumn: "Id",
                keyValue: new Guid("9c4c2c9c-4c2e-433f-8cb5-2ba74dcdf7b7"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("1ef91520-f995-4871-8d61-11386ae17d5f"));

            migrationBuilder.DeleteData(
                table: "KnowledgeTypes",
                keyColumn: "Id",
                keyValue: new Guid("ae87764f-e6c7-44d5-ad32-2fb91fc8ce32"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("3a95146b-6106-4fe6-998f-98a59791dd5f"));

            migrationBuilder.DeleteData(
                table: "Learnings",
                keyColumn: "Id",
                keyValue: new Guid("735f8cd5-0bde-4275-b0d5-3733362eca09"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("4b8864c1-8755-4d73-8b7b-00cdce57a74a"));

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: new Guid("a2c7d934-b843-49b8-9d0f-3daa10c98697"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("0a14ada4-c9a8-47ce-bd74-9e889e773804"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("595dcfd0-d177-4ff4-ab53-fbe378d32468"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"));

            migrationBuilder.DeleteData(
                table: "Knowledges",
                keyColumn: "Id",
                keyValue: new Guid("60f522b1-69a5-46c2-9487-098d79e60741"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("0a14ada4-c9a8-47ce-bd74-9e889e773804"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5200), "Description 1", "image1.jpg", "Game 1" },
                    { new Guid("595dcfd0-d177-4ff4-ab53-fbe378d32468"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5240), "Description 2", "image2.jpg", "Game 2" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title" },
                values: new object[,]
                {
                    { new Guid("5e7aa25a-3b50-449a-a9da-443d8174e1aa"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4680), 1, null, "Algebra" },
                    { new Guid("9c4c2c9c-4c2e-433f-8cb5-2ba74dcdf7b7"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4690), 2, null, "Physics" }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("1ef91520-f995-4871-8d61-11386ae17d5f"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4640), "Theory", null },
                    { new Guid("ae87764f-e6c7-44d5-ad32-2fb91fc8ce32"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4650), "Practical", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Photo" },
                values: new object[,]
                {
                    { new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4450), "Study of the physical and natural world.", "Science", "test.png" },
                    { new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4440), "Study of numbers, shapes, and patterns.", "Mathematics", "test.png" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("4b8864c1-8755-4d73-8b7b-00cdce57a74a"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4490), "A track focused on Mathematics.", "Mathematics Track" },
                    { new Guid("a2c7d934-b843-49b8-9d0f-3daa10c98697"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4590), "A track focused on Science.", "Science Track" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UserName" },
                values: new object[] { new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(3710), "testuser@example.com", null, 1, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UserId" },
                values: new object[] { new Guid("73634806-5e73-411c-afcc-361bd3fd972e"), null, null, new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4010), "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", true, true, null, null, new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "Visibility" },
                values: new object[,]
                {
                    { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4730), new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56"), 0, "Introduction to Algebra", 0 },
                    { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4740), new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56"), 2, "Introduction to Physics", 0 }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e"), new Guid("4b8864c1-8755-4d73-8b7b-00cdce57a74a"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4850) },
                    { new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e"), new Guid("a2c7d934-b843-49b8-9d0f-3daa10c98697"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4850) }
                });

            migrationBuilder.InsertData(
                table: "GameKnowledgeSubscriptions",
                columns: new[] { "Id", "CreatedAt", "GameId", "KnowledgeId", "ModifiedAt", "ModifiedBy" },
                values: new object[,]
                {
                    { new Guid("1dcf2b12-8b2c-4d8e-b992-261fdf07e5c9"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5280), new Guid("0a14ada4-c9a8-47ce-bd74-9e889e773804"), new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), null, null },
                    { new Guid("b7811e7a-6df6-4d7d-a81f-f873168f0f9b"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5290), new Guid("595dcfd0-d177-4ff4-ab53-fbe378d32468"), new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), null, null }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("5e7aa25a-3b50-449a-a9da-443d8174e1aa"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5160) },
                    { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("9c4c2c9c-4c2e-433f-8cb5-2ba74dcdf7b7"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5170) }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("1ef91520-f995-4871-8d61-11386ae17d5f"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5120) },
                    { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("ae87764f-e6c7-44d5-ad32-2fb91fc8ce32"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5130) }
                });

            migrationBuilder.InsertData(
                table: "Learnings",
                columns: new[] { "Id", "CreatedAt", "KnowledgeId", "ModifiedAt", "ModifiedBy", "NextReviewDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("3a95146b-6106-4fe6-998f-98a59791dd5f"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5400), new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56") },
                    { new Guid("735f8cd5-0bde-4275-b0d5-3733362eca09"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5410), new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("70bfb17c-d2cc-48f0-a071-4aa35c746d56") }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type" },
                values: new object[,]
                {
                    { new Guid("b5ce233f-83a6-4424-8301-81f852ae0b69"), "Video content about Algebra.", new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4800), new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), 1, null, 4 },
                    { new Guid("d6d7f7c7-a66f-4281-942d-a1c8793449f0"), "Article about Physics.", new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(4810), new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), 2, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("60f522b1-69a5-46c2-9487-098d79e60741"), new Guid("3a0dcf72-d5c6-4550-bfea-e15f392e243e"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5010) },
                    { new Guid("2ec6e41b-faba-4ed1-bc29-f7f8ea5340d6"), new Guid("ec8f31c6-e6b9-41ee-b5a8-1eee3970898e"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5000) }
                });

            migrationBuilder.InsertData(
                table: "GameOptions",
                columns: new[] { "Id", "CreatedAt", "GameKnowledgeSubscriptionId", "Group", "IsCorrect", "Order", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("00b4f54f-ea0b-4a42-8b35-562ab18fc60f"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5330), new Guid("1dcf2b12-8b2c-4d8e-b992-261fdf07e5c9"), 1, true, null, 1, "Value 1" },
                    { new Guid("38465583-45fb-4e13-bc31-261fbee57e75"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5340), new Guid("1dcf2b12-8b2c-4d8e-b992-261fdf07e5c9"), 1, null, null, 1, "Wrong Value" },
                    { new Guid("410a3ef3-91d3-4790-94c7-2c34599cc36b"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5330), new Guid("1dcf2b12-8b2c-4d8e-b992-261fdf07e5c9"), 1, null, null, 0, "What is Value 1?" },
                    { new Guid("77ab178b-132d-4b58-b46f-8fada61cd3c0"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5350), new Guid("b7811e7a-6df6-4d7d-a81f-f873168f0f9b"), 2, null, null, 0, "What is Value 2?" },
                    { new Guid("802cd8b9-4e45-47b1-a462-6f21d7cb69ee"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5360), new Guid("b7811e7a-6df6-4d7d-a81f-f873168f0f9b"), 2, null, null, 1, "Value 2" },
                    { new Guid("bfa7c1d4-b42b-4cce-9729-8b1b7f315bb6"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5350), new Guid("b7811e7a-6df6-4d7d-a81f-f873168f0f9b"), 2, true, null, 1, "Value 2" }
                });

            migrationBuilder.InsertData(
                table: "LearningHistories",
                columns: new[] { "Id", "CreatedAt", "IsMemorized", "LearningId", "LearningLevel", "PlayedGameId", "Score" },
                values: new object[,]
                {
                    { new Guid("1737db4c-21f0-44d3-bdbc-9b2c4541a310"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5540), false, new Guid("735f8cd5-0bde-4275-b0d5-3733362eca09"), 1, new Guid("595dcfd0-d177-4ff4-ab53-fbe378d32468"), 80 },
                    { new Guid("e8983958-c302-4907-b6ba-4cd3d6428b1d"), new DateTime(2024, 11, 14, 6, 49, 26, 941, DateTimeKind.Utc).AddTicks(5450), true, new Guid("3a95146b-6106-4fe6-998f-98a59791dd5f"), 0, new Guid("0a14ada4-c9a8-47ce-bd74-9e889e773804"), 100 }
                });
        }
    }
}
