using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationWithKnowledge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                name: "KnowledgeTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeTopics_KnowledgeTopics_ParentId",
                        column: x => x.ParentId,
                        principalTable: "KnowledgeTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeTypes_KnowledgeTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "KnowledgeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgeTopicKnowledges",
                columns: table => new
                {
                    KnowledgeTopicId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                name: "SubjectKnowledges",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KnowledgeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                name: "TrackSubjects",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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

            migrationBuilder.InsertData(
                table: "KnowledgeTopics",
                columns: new[] { "Id", "CreatedAt", "Order", "ParentId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4fe3d034-dc7a-49a8-8cf8-530e5be289e7"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2080), 1, null, "Algebra", null },
                    { new Guid("ffd9dbcb-d79a-478e-90d0-f851dfaa327a"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2080), 2, null, "Physics", null }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("032c17e2-a09f-496d-b5d4-e09cd3908bde"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1950), "Practical", null, null },
                    { new Guid("8079145e-1c79-4103-ab26-b05b2e4b0369"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1940), "Theory", null, null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2d054a42-8694-465d-8971-189f7fda3d7d"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1880), "Study of the physical and natural world.", "Science", null },
                    { new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1870), "Study of numbers, shapes, and patterns.", "Mathematics", null }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("56a5c104-ba0d-4085-b643-df85f9abf418"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1910), "A track focused on Science.", "Science Track", null },
                    { new Guid("8f82cf34-e348-41b4-9644-db3601afce61"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1910), "A track focused on Mathematics.", "Mathematics Track", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PhotoUrl", "Role", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1500), "testuser@example.com", null, 1, null, "testuser" });

            migrationBuilder.InsertData(
                table: "Authentications",
                columns: new[] { "Id", "ConfirmationCode", "ConfirmationCodeExpiryTime", "CreatedAt", "HashedPassword", "IsActivated", "IsEmailConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("325598c0-2790-47fe-bc06-3ab6a61f852e"), null, null, new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1830), "hashedpassword", true, true, null, null, null, new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9") });

            migrationBuilder.InsertData(
                table: "Knowledges",
                columns: new[] { "Id", "CreatedAt", "CreatorId", "Level", "Title", "UpdatedAt", "Visibility" },
                values: new object[,]
                {
                    { new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2120), new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"), 0, "Introduction to Algebra", null, 0 },
                    { new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"), new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2130), new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"), 0, "Introduction to Physics", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "TrackSubjects",
                columns: new[] { "SubjectId", "TrackId" },
                values: new object[,]
                {
                    { new Guid("2d054a42-8694-465d-8971-189f7fda3d7d"), new Guid("56a5c104-ba0d-4085-b643-df85f9abf418") },
                    { new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d"), new Guid("8f82cf34-e348-41b4-9644-db3601afce61") }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTopicKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTopicId" },
                values: new object[,]
                {
                    { new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"), new Guid("4fe3d034-dc7a-49a8-8cf8-530e5be289e7") },
                    { new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"), new Guid("ffd9dbcb-d79a-478e-90d0-f851dfaa327a") }
                });

            migrationBuilder.InsertData(
                table: "KnowledgeTypeKnowledges",
                columns: new[] { "KnowledgeId", "KnowledgeTypeId" },
                values: new object[,]
                {
                    { new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"), new Guid("032c17e2-a09f-496d-b5d4-e09cd3908bde") },
                    { new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"), new Guid("8079145e-1c79-4103-ab26-b05b2e4b0369") }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Content", "CreatedAt", "KnowledgeId", "Order", "ParentId", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2afe8d45-1afb-41dd-bcdd-410f9eea35c4"), "Article about Physics.", new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2170), new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"), 2, null, 0, null },
                    { new Guid("9b206b9f-e1d7-48a4-aa0c-be3cf05598b0"), "Video content about Algebra.", new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2160), new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"), 1, null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "SubjectKnowledges",
                columns: new[] { "KnowledgeId", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"), new Guid("2d054a42-8694-465d-8971-189f7fda3d7d") },
                    { new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"), new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d") }
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgeTopicKnowledges");

            migrationBuilder.DropTable(
                name: "KnowledgeTypeKnowledges");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "SubjectKnowledges");

            migrationBuilder.DropTable(
                name: "TrackSubjects");

            migrationBuilder.DropTable(
                name: "KnowledgeTopics");

            migrationBuilder.DropTable(
                name: "KnowledgeTypes");

            migrationBuilder.DropTable(
                name: "Knowledges");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DeleteData(
                table: "Authentications",
                keyColumn: "Id",
                keyValue: new Guid("325598c0-2790-47fe-bc06-3ab6a61f852e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"));
        }
    }
}
