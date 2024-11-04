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

            modelBuilder.Entity("Domain.Entities.PivotEntities.KnowledgeTopicKnowledge", b =>
                {
                    b.Property<Guid>("KnowledgeTopicId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("KnowledgeTopicId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("KnowledgeTopicKnowledges");

                    b.HasData(
                        new
                        {
                            KnowledgeTopicId = new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c"),
                            KnowledgeId = new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8370)
                        },
                        new
                        {
                            KnowledgeTopicId = new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656"),
                            KnowledgeId = new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8370)
                        });
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.KnowledgeTypeKnowledge", b =>
                {
                    b.Property<Guid>("KnowledgeTypeId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("KnowledgeTypeId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("KnowledgeTypeKnowledges");

                    b.HasData(
                        new
                        {
                            KnowledgeTypeId = new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e"),
                            KnowledgeId = new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8350)
                        },
                        new
                        {
                            KnowledgeTypeId = new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc"),
                            KnowledgeId = new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8350)
                        });
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.SubjectKnowledge", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("KnowledgeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("SubjectId", "KnowledgeId");

                    b.HasIndex("KnowledgeId");

                    b.ToTable("SubjectKnowledges");

                    b.HasData(
                        new
                        {
                            SubjectId = new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"),
                            KnowledgeId = new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8320)
                        },
                        new
                        {
                            SubjectId = new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"),
                            KnowledgeId = new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8330)
                        });
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.TrackSubject", b =>
                {
                    b.Property<Guid>("TrackId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("TrackId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("TrackSubjects");

                    b.HasData(
                        new
                        {
                            TrackId = new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67"),
                            SubjectId = new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8300)
                        },
                        new
                        {
                            TrackId = new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb"),
                            SubjectId = new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8300)
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Authentication", b =>
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

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Authentications");

                    b.HasData(
                        new
                        {
                            Id = new Guid("45007bfc-f89a-4c89-8534-8fef4b26c41f"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7600),
                            HashedPassword = "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=",
                            IsActivated = true,
                            IsEmailConfirmed = true,
                            UserId = new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66")
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Knowledge", b =>
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

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Knowledges", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8210),
                            CreatorId = new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"),
                            Level = 0,
                            Title = "Introduction to Algebra",
                            Visibility = 0
                        },
                        new
                        {
                            Id = new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8220),
                            CreatorId = new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"),
                            Level = 0,
                            Title = "Introduction to Physics",
                            Visibility = 0
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeTopic", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("KnowledgeTopics", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("55b60279-c67d-44a6-bfe5-f2c409fcfb4c"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8090),
                            Order = 1,
                            Title = "Algebra"
                        },
                        new
                        {
                            Id = new Guid("efd02e3b-c2f7-4b9b-88bc-191eb4c6c656"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8100),
                            Order = 2,
                            Title = "Physics"
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeType", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("KnowledgeTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4fded59d-ec8f-4490-b929-f2b6c5e30d2e"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8050),
                            Name = "Theory"
                        },
                        new
                        {
                            Id = new Guid("2f61b74d-2d91-49c7-b57b-1d06d70898dc"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8060),
                            Name = "Practical"
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Material", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("KnowledgeId");

                    b.HasIndex("ParentId");

                    b.ToTable("Materials", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("b1c8a2be-195e-4358-bc52-3a52bea54315"),
                            Content = "Video content about Algebra.",
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8250),
                            KnowledgeId = new Guid("cae8dd59-9b2e-4254-94a7-f1ef79659247"),
                            Order = 1,
                            Type = 2
                        },
                        new
                        {
                            Id = new Guid("53f2fa8f-29df-4adc-b323-3fb61f70293f"),
                            Content = "Article about Physics.",
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8260),
                            KnowledgeId = new Guid("bebc79bf-9bf8-42f6-aa47-25eb544db980"),
                            Order = 2,
                            Type = 0
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Subject", b =>
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

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Subjects", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("d2c5ba4c-d530-423f-b719-bb81de10a6ec"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7970),
                            Description = "Study of numbers, shapes, and patterns.",
                            Name = "Mathematics",
                            Photo = "test.png"
                        },
                        new
                        {
                            Id = new Guid("4a09133b-437a-4607-a0b9-7bc87db688d8"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7980),
                            Description = "Study of the physical and natural world.",
                            Name = "Science",
                            Photo = "test.png"
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Track", b =>
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

                    b.HasKey("Id");

                    b.ToTable("Tracks", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("ab9f078a-116d-4109-9bef-b54174f3ca67"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8010),
                            Description = "A track focused on Mathematics.",
                            Name = "Mathematics Track"
                        },
                        new
                        {
                            Id = new Guid("74eac258-0b34-4c20-bad8-100b66ac1ffb"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(8020),
                            Description = "A track focused on Science.",
                            Name = "Science Track"
                        });
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.User", b =>
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
                            Id = new Guid("0cd8c681-cfc5-490c-a4de-d0c61637cd66"),
                            CreatedAt = new DateTime(2024, 11, 4, 10, 1, 24, 808, DateTimeKind.Utc).AddTicks(7300),
                            Email = "testuser@example.com",
                            Role = 1,
                            UserName = "testuser"
                        });
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.KnowledgeTopicKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.Knowledge", "Knowledge")
                        .WithMany("KnowledgeTopicKnowledges")
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.SingleIdEntities.KnowledgeTopic", "KnowledgeTopic")
                        .WithMany("KnowledgeTopicKnowledges")
                        .HasForeignKey("KnowledgeTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Knowledge");

                    b.Navigation("KnowledgeTopic");
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.KnowledgeTypeKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.Knowledge", "Knowledge")
                        .WithMany("KnowledgeTypeKnowledges")
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.SingleIdEntities.KnowledgeType", "KnowledgeType")
                        .WithMany("KnowledgeTypeKnowledges")
                        .HasForeignKey("KnowledgeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Knowledge");

                    b.Navigation("KnowledgeType");
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.SubjectKnowledge", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.Knowledge", "Knowledge")
                        .WithMany("SubjectKnowledges")
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.SingleIdEntities.Subject", "Subject")
                        .WithMany("SubjectKnowledges")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Knowledge");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Domain.Entities.PivotEntities.TrackSubject", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.Subject", "Subject")
                        .WithMany("TrackSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.SingleIdEntities.Track", "Track")
                        .WithMany("TrackSubjects")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Authentication", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.User", "User")
                        .WithOne("Authentication")
                        .HasForeignKey("Domain.Entities.SingleIdEntities.Authentication", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Knowledge", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeTopic", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.KnowledgeTopic", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeType", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.KnowledgeType", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Material", b =>
                {
                    b.HasOne("Domain.Entities.SingleIdEntities.Knowledge", "Knowledge")
                        .WithMany("Materials")
                        .HasForeignKey("KnowledgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.SingleIdEntities.Material", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Knowledge");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Knowledge", b =>
                {
                    b.Navigation("KnowledgeTopicKnowledges");

                    b.Navigation("KnowledgeTypeKnowledges");

                    b.Navigation("Materials");

                    b.Navigation("SubjectKnowledges");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeTopic", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("KnowledgeTopicKnowledges");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.KnowledgeType", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("KnowledgeTypeKnowledges");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Material", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Subject", b =>
                {
                    b.Navigation("SubjectKnowledges");

                    b.Navigation("TrackSubjects");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.Track", b =>
                {
                    b.Navigation("TrackSubjects");
                });

            modelBuilder.Entity("Domain.Entities.SingleIdEntities.User", b =>
                {
                    b.Navigation("Authentication");
                });
#pragma warning restore 612, 618
        }
    }
}
