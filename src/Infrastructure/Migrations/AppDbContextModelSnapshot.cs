﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Authentication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ConfirmationCode")
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.Property<DateTime?>("ConfirmationCodeExpiryTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Authentications");

                    b.HasData(
                        new
                        {
                            Id = new Guid("325598c0-2790-47fe-bc06-3ab6a61f852e"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1830),
                            HashedPassword = "hashedpassword",
                            IsActivated = true,
                            IsEmailConfirmed = true,
                            UserId = new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9")
                        });
                });

            modelBuilder.Entity("Domain.Entities.Knowledge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Knowledges", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2120),
                            CreatorId = new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"),
                            Level = 0,
                            Title = "Introduction to Algebra",
                            Visibility = 0
                        },
                        new
                        {
                            Id = new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2130),
                            CreatorId = new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"),
                            Level = 0,
                            Title = "Introduction to Physics",
                            Visibility = 0
                        });
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeTopic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("KnowledgeTopics", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4fe3d034-dc7a-49a8-8cf8-530e5be289e7"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2080),
                            Order = 1,
                            Title = "Algebra"
                        },
                        new
                        {
                            Id = new Guid("ffd9dbcb-d79a-478e-90d0-f851dfaa327a"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2080),
                            Order = 2,
                            Title = "Physics"
                        });
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("KnowledgeTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8079145e-1c79-4103-ab26-b05b2e4b0369"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1940),
                            Name = "Theory"
                        },
                        new
                        {
                            Id = new Guid("032c17e2-a09f-496d-b5d4-e09cd3908bde"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1950),
                            Name = "Practical"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("KnowledgeId");

                    b.HasIndex("ParentId");

                    b.ToTable("Materials", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("9b206b9f-e1d7-48a4-aa0c-be3cf05598b0"),
                            Content = "Video content about Algebra.",
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2160),
                            KnowledgeId = new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c"),
                            Order = 1,
                            Type = 2
                        },
                        new
                        {
                            Id = new Guid("2afe8d45-1afb-41dd-bcdd-410f9eea35c4"),
                            Content = "Article about Physics.",
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(2170),
                            KnowledgeId = new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62"),
                            Order = 2,
                            Type = 0
                        });
                });

            modelBuilder.Entity("Domain.Entities.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Subjects", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1870),
                            Description = "Study of numbers, shapes, and patterns.",
                            Name = "Mathematics"
                        },
                        new
                        {
                            Id = new Guid("2d054a42-8694-465d-8971-189f7fda3d7d"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1880),
                            Description = "Study of the physical and natural world.",
                            Name = "Science"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Track", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Tracks", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("8f82cf34-e348-41b4-9644-db3601afce61"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1910),
                            Description = "A track focused on Mathematics.",
                            Name = "Mathematics Track"
                        },
                        new
                        {
                            Id = new Guid("56a5c104-ba0d-4085-b643-df85f9abf418"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1910),
                            Description = "A track focused on Science.",
                            Name = "Science Track"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ab2cc52e-3ce5-4321-9d18-a5f69149edc9"),
                            CreatedAt = new DateTime(2024, 10, 28, 9, 52, 38, 954, DateTimeKind.Utc).AddTicks(1500),
                            Email = "testuser@example.com",
                            Role = 1,
                            UserName = "testuser"
                        });
                });

            modelBuilder.Entity("KnowledgeTopicKnowledge", b =>
                {
                    b.Property<Guid>("KnowledgeTopicId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.HasKey("KnowledgeTopicId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("KnowledgeTopicKnowledges", (string)null);

                    b.HasData(
                        new
                        {
                            KnowledgeTopicId = new Guid("4fe3d034-dc7a-49a8-8cf8-530e5be289e7"),
                            KnowledgeId = new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c")
                        },
                        new
                        {
                            KnowledgeTopicId = new Guid("ffd9dbcb-d79a-478e-90d0-f851dfaa327a"),
                            KnowledgeId = new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62")
                        });
                });

            modelBuilder.Entity("KnowledgeTypeKnowledge", b =>
                {
                    b.Property<Guid>("KnowledgeTypeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.HasKey("KnowledgeTypeId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("KnowledgeTypeKnowledges", (string)null);

                    b.HasData(
                        new
                        {
                            KnowledgeTypeId = new Guid("8079145e-1c79-4103-ab26-b05b2e4b0369"),
                            KnowledgeId = new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c")
                        },
                        new
                        {
                            KnowledgeTypeId = new Guid("032c17e2-a09f-496d-b5d4-e09cd3908bde"),
                            KnowledgeId = new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62")
                        });
                });

            modelBuilder.Entity("SubjectKnowledge", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.HasKey("SubjectId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("SubjectKnowledges", (string)null);

                    b.HasData(
                        new
                        {
                            SubjectId = new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d"),
                            KnowledgeId = new Guid("4b50391a-3fb0-4e2c-a00e-d2d13a094c5c")
                        },
                        new
                        {
                            SubjectId = new Guid("2d054a42-8694-465d-8971-189f7fda3d7d"),
                            KnowledgeId = new Guid("8feaa5d3-6321-4a41-b8ab-cc76d43bed62")
                        });
                });

            modelBuilder.Entity("TrackSubject", b =>
                {
                    b.Property<Guid>("TrackId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("char(36)");

                    b.HasKey("TrackId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("TrackSubjects", (string)null);

                    b.HasData(
                        new
                        {
                            TrackId = new Guid("8f82cf34-e348-41b4-9644-db3601afce61"),
                            SubjectId = new Guid("bb2b74be-daf3-49f3-b54b-a67c6ead109d")
                        },
                        new
                        {
                            TrackId = new Guid("56a5c104-ba0d-4085-b643-df85f9abf418"),
                            SubjectId = new Guid("2d054a42-8694-465d-8971-189f7fda3d7d")
                        });
                });

            modelBuilder.Entity("Domain.Entities.Authentication", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne("Authentication")
                        .HasForeignKey("Domain.Entities.Authentication", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Knowledge", b =>
                {
                    b.HasOne("Domain.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeTopic", b =>
                {
                    b.HasOne("Domain.Entities.KnowledgeTopic", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeType", b =>
                {
                    b.HasOne("Domain.Entities.KnowledgeType", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.Material", b =>
                {
                    b.HasOne("Domain.Entities.Knowledge", "Knowledge")
                        .WithMany("Materials")
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Material", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Knowledge");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("KnowledgeTopicKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.Knowledge", null)
                        .WithMany()
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.KnowledgeTopic", null)
                        .WithMany()
                        .HasForeignKey("KnowledgeTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KnowledgeTypeKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.Knowledge", null)
                        .WithMany()
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.KnowledgeType", null)
                        .WithMany()
                        .HasForeignKey("KnowledgeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SubjectKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.Knowledge", null)
                        .WithMany()
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackSubject", b =>
                {
                    b.HasOne("Domain.Entities.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Track", null)
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Knowledge", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeTopic", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeType", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Domain.Entities.Material", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Authentication");
                });
#pragma warning restore 612, 618
        }
    }
}
