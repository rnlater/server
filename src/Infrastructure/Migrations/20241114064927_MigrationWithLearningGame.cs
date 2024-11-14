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
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgeTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeTopics_KnowledgeTopics_ParentId",
                        column: x => x.ParentId,
                        principalTable: "KnowledgeTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeTypes_KnowledgeTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "KnowledgeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackSubjects",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackSubjects", x => new { x.TrackId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_TrackSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackSubjects_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authentications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    HashedPassword = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConfirmationCode = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfirmationCodeExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsEmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActivated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authentications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Knowledges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Knowledges_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameKnowledgeSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameKnowledgeSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameKnowledgeSubscriptions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameKnowledgeSubscriptions_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgeTopicKnowledges",
                columns: table => new
                {
                    KnowledgeTopicId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTopicKnowledges", x => new { x.KnowledgeTopicId, x.KnowledgeId });
                    table.ForeignKey(
                        name: "FK_KnowledgeTopicKnowledges_KnowledgeTopics_KnowledgeTopicId",
                        column: x => x.KnowledgeTopicId,
                        principalTable: "KnowledgeTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeTopicKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgeTypeKnowledges",
                columns: table => new
                {
                    KnowledgeTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTypeKnowledges", x => new { x.KnowledgeTypeId, x.KnowledgeId });
                    table.ForeignKey(
                        name: "FK_KnowledgeTypeKnowledges_KnowledgeTypes_KnowledgeTypeId",
                        column: x => x.KnowledgeTypeId,
                        principalTable: "KnowledgeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeTypeKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Learnings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NextReviewDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Learnings_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Learnings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_Materials_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubjectKnowledges",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectKnowledges", x => new { x.SubjectId, x.KnowledgeId });
                    table.ForeignKey(
                        name: "FK_SubjectKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectKnowledges_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameKnowledgeSubscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Group = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameOptions_GameKnowledgeSubscriptions_GameKnowledgeSubscrip~",
                        column: x => x.GameKnowledgeSubscriptionId,
                        principalTable: "GameKnowledgeSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LearningId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LearningLevel = table.Column<int>(type: "int", nullable: false),
                    IsMemorized = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PlayedGameId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningHistories_Games_PlayedGameId",
                        column: x => x.PlayedGameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningHistories_Learnings_LearningId",
                        column: x => x.LearningId,
                        principalTable: "Learnings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_Authentications_UserId",
                table: "Authentications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameKnowledgeSubscriptions_GameId",
                table: "GameKnowledgeSubscriptions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameKnowledgeSubscriptions_KnowledgeId",
                table: "GameKnowledgeSubscriptions",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameOptions_GameKnowledgeSubscriptionId",
                table: "GameOptions",
                column: "GameKnowledgeSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Knowledges_CreatorId",
                table: "Knowledges",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeTopicKnowledges_KnowledgeId",
                table: "KnowledgeTopicKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeTopics_ParentId",
                table: "KnowledgeTopics",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeTypeKnowledges_KnowledgeId",
                table: "KnowledgeTypeKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeTypes_ParentId",
                table: "KnowledgeTypes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningHistories_LearningId",
                table: "LearningHistories",
                column: "LearningId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningHistories_PlayedGameId",
                table: "LearningHistories",
                column: "PlayedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Learnings_KnowledgeId",
                table: "Learnings",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Learnings_UserId",
                table: "Learnings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_KnowledgeId",
                table: "Materials",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ParentId",
                table: "Materials",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectKnowledges_KnowledgeId",
                table: "SubjectKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSubjects_SubjectId",
                table: "TrackSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authentications");

            migrationBuilder.DropTable(
                name: "GameOptions");

            migrationBuilder.DropTable(
                name: "KnowledgeTopicKnowledges");

            migrationBuilder.DropTable(
                name: "KnowledgeTypeKnowledges");

            migrationBuilder.DropTable(
                name: "LearningHistories");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "SubjectKnowledges");

            migrationBuilder.DropTable(
                name: "TrackSubjects");

            migrationBuilder.DropTable(
                name: "GameKnowledgeSubscriptions");

            migrationBuilder.DropTable(
                name: "KnowledgeTopics");

            migrationBuilder.DropTable(
                name: "KnowledgeTypes");

            migrationBuilder.DropTable(
                name: "Learnings");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Knowledges");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
