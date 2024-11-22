using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region Properties

        public required DbSet<User> Users { get; set; }
        public required DbSet<Authentication> Authentications { get; set; }
        public required DbSet<Knowledge> Knowledges { get; set; }
        public required DbSet<KnowledgeTopic> KnowledgeTopics { get; set; }
        public required DbSet<KnowledgeType> KnowledgeTypes { get; set; }
        public required DbSet<Material> Materials { get; set; }
        public required DbSet<Subject> Subjects { get; set; }
        public required DbSet<Track> Tracks { get; set; }
        public required DbSet<TrackSubject> TrackSubjects { get; set; }
        public required DbSet<SubjectKnowledge> SubjectKnowledges { get; set; }
        public required DbSet<KnowledgeTypeKnowledge> KnowledgeTypeKnowledges { get; set; }
        public required DbSet<KnowledgeTopicKnowledge> KnowledgeTopicKnowledges { get; set; }
        public required DbSet<Learning> Learnings { get; set; }
        public required DbSet<LearningHistory> LearningHistories { get; set; }
        public required DbSet<Game> Games { get; set; }
        public required DbSet<GameOption> GameOptions { get; set; }
        public required DbSet<GameKnowledgeSubscription> GameKnowledgeSubscriptions { get; set; }
        public required DbSet<LearningList> LearningLists { get; set; }
        public required DbSet<LearningListKnowledge> LearningListKnowledges { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUserEntity(modelBuilder);
            ConfigureAuthenticationEntity(modelBuilder);
            ConfigureKnowledgeEntity(modelBuilder);
            ConfigureKnowledgeTopicEntity(modelBuilder);
            ConfigureKnowledgeTypeEntity(modelBuilder);
            ConfigureMaterialEntity(modelBuilder);
            ConfigureSubjectEntity(modelBuilder);
            ConfigureTrackEntity(modelBuilder);
            ConfigureTrackSubjectEntity(modelBuilder);
            ConfigureSubjectKnowledgeEntity(modelBuilder);
            ConfigureKnowledgeTypeKnowledgeEntity(modelBuilder);
            ConfigureKnowledgeTopicKnowledgeEntity(modelBuilder);
            ConfigureGameEntity(modelBuilder);
            ConfigureGameOptionEntity(modelBuilder);
            ConfigureGameKnowledgeSubscriptionEntity(modelBuilder);
            ConfigureLearningEntity(modelBuilder);
            ConfigureLearningHistoryEntity(modelBuilder);
            ConfigureLearningListEntity(modelBuilder);
            ConfigureLearningListKnowledgeEntity(modelBuilder);

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(SeedData.GetUsers());
            modelBuilder.Entity<Authentication>().HasData(SeedData.GetAuthentications());
            modelBuilder.Entity<Subject>().HasData(SeedData.GetSubjects());
            modelBuilder.Entity<Track>().HasData(SeedData.GetTracks());
            modelBuilder.Entity<KnowledgeType>().HasData(SeedData.GetKnowledgeTypes());
            modelBuilder.Entity<KnowledgeTopic>().HasData(SeedData.GetKnowledgeTopics());
            modelBuilder.Entity<Knowledge>().HasData(SeedData.GetKnowledges());
            modelBuilder.Entity<Material>().HasData(SeedData.GetMaterials());
            modelBuilder.Entity<TrackSubject>().HasData(SeedData.GetTrackSubjects());
            modelBuilder.Entity<SubjectKnowledge>().HasData(SeedData.GetSubjectKnowledges());
            modelBuilder.Entity<KnowledgeTypeKnowledge>().HasData(SeedData.GetKnowledgeTypeKnowledges());
            modelBuilder.Entity<KnowledgeTopicKnowledge>().HasData(SeedData.GetKnowledgeTopicKnowledges());
            modelBuilder.Entity<Game>().HasData(SeedData.GetGames());
            modelBuilder.Entity<GameKnowledgeSubscription>().HasData(SeedData.GetGameKnowledgeSubscriptions());
            modelBuilder.Entity<GameOption>().HasData(SeedData.GetGameOptions());
            modelBuilder.Entity<Learning>().HasData(SeedData.GetLearnings());
            modelBuilder.Entity<LearningHistory>().HasData(SeedData.GetLearningHistories());
            modelBuilder.Entity<LearningList>().HasData(SeedData.GetLearningLists());
            modelBuilder.Entity<LearningListKnowledge>().HasData(SeedData.GetLearningListKnowledges());
        }

        #region ConfigureEntities
        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.Email).IsRequired();
            });
        }

        private void ConfigureAuthenticationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.HashedPassword).IsRequired();
                entity.Property(a => a.IsEmailConfirmed).IsRequired();
                entity.Property(a => a.IsActivated).IsRequired();
                entity.HasOne(a => a.User)
                      .WithOne(u => u.Authentication)
                      .HasForeignKey<Authentication>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureKnowledgeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Knowledge>(b =>
            {
                b.HasKey(k => k.Id);
                b.Property(k => k.Title)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(k => k.Visibility)
                    .IsRequired()
                    .HasColumnType("int");
                b.Property(k => k.Level)
                    .IsRequired()
                    .HasColumnType("int");
                b.Property(k => k.CreatorId)
                    .IsRequired()
                    .HasColumnType("char(36)");

                b.HasOne(k => k.Creator)
                    .WithMany()
                    .HasForeignKey(k => k.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.ToTable("Knowledges");
            });
        }

        private void ConfigureKnowledgeTopicEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeTopic>(b =>
            {
                b.HasKey(kt => kt.Id);
                b.Property(kt => kt.Title)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(kt => kt.Order)
                    .IsRequired(false)
                    .HasColumnType("int");
                b.Property(kt => kt.ParentId)
                    .HasColumnType("char(36)");

                b.HasOne(kt => kt.Parent)
                    .WithMany(kt => kt.Children)
                    .HasForeignKey(kt => kt.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.ToTable("KnowledgeTopics");
            });
        }

        private void ConfigureKnowledgeTypeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeType>(b =>
            {
                b.HasKey(kt => kt.Id);
                b.Property(kt => kt.Name)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(kt => kt.ParentId)
                    .HasColumnType("char(36)");

                b.HasOne(kt => kt.Parent)
                    .WithMany(kt => kt.Children)
                    .HasForeignKey(kt => kt.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.ToTable("KnowledgeTypes");
            });
        }

        private void ConfigureMaterialEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Type)
                    .IsRequired()
                    .HasColumnType("int");
                b.Property(m => m.Content)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(m => m.Order)
                    .IsRequired(false)
                    .HasColumnType("int");
                b.Property(m => m.KnowledgeId)
                    .IsRequired()
                    .HasColumnType("char(36)");
                b.Property(m => m.ParentId)
                    .HasColumnType("char(36)");

                b.HasOne(m => m.Knowledge)
                    .WithMany(k => k.Materials)
                    .HasForeignKey(m => m.KnowledgeId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(m => m.Parent)
                    .WithMany(m => m.Children)
                    .HasForeignKey(m => m.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.ToTable("Materials");
            });
        }

        private void ConfigureSubjectEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Name)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(s => s.Description)
                    .IsRequired()
                    .HasColumnType("longtext");

                b.ToTable("Subjects");
            });
        }

        private void ConfigureTrackEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Name)
                    .IsRequired()
                    .HasColumnType("longtext");
                b.Property(t => t.Description)
                    .IsRequired()
                    .HasColumnType("longtext");

                b.ToTable("Tracks");
            });
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired();
                entity.Property(g => g.Description).IsRequired();
                entity.Property(g => g.ImageUrl).IsRequired();
            });
        }

        private void ConfigureGameOptionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameOption>(entity =>
            {
                entity.HasKey(go => go.Id);
                entity.Property(go => go.Type).IsRequired();
                entity.Property(go => go.Value).IsRequired();
                entity.HasOne(go => go.GameKnowledgeSubscription)
                    .WithMany(gks => gks.GameOptions)
                    .HasForeignKey(go => go.GameKnowledgeSubscriptionId);
            });
        }

        private void ConfigureGameKnowledgeSubscriptionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameKnowledgeSubscription>(entity =>
            {
                entity.HasKey(gks => gks.Id);
                entity.HasOne(gks => gks.Game)
                    .WithMany(g => g.GameKnowledgeSubscriptions)
                    .HasForeignKey(gks => gks.GameId);
                entity.HasOne(gks => gks.Knowledge)
                    .WithMany(k => k.GameKnowledgeSubscriptions)
                    .HasForeignKey(gks => gks.KnowledgeId);
            });
        }

        private void ConfigureLearningEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Learning>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.HasOne(l => l.Knowledge)
                    .WithMany(k => k.Learnings)
                    .HasForeignKey(l => l.KnowledgeId);
                entity.HasOne(l => l.User)
                    .WithMany(u => u.Learnings)
                    .HasForeignKey(l => l.UserId);
            });
        }

        private void ConfigureLearningHistoryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningHistory>(entity =>
            {
                entity.HasKey(lh => lh.Id);
                entity.Property(lh => lh.LearningLevel).IsRequired();
                entity.Property(lh => lh.IsMemorized).IsRequired();
                entity.Property(lh => lh.Score).IsRequired();
                entity.HasOne(lh => lh.Learning)
                    .WithMany(l => l.LearningHistories)
                    .HasForeignKey(lh => lh.LearningId);
                entity.HasOne(lh => lh.PlayedGame)
                    .WithMany()
                    .HasForeignKey(lh => lh.PlayedGameId);
            });
        }

        private void ConfigureTrackSubjectEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackSubject>()
            .HasKey(ts => new { ts.TrackId, ts.SubjectId });
            modelBuilder.Entity<TrackSubject>()
            .HasOne(ts => ts.Track)
            .WithMany(t => t.TrackSubjects)
            .HasForeignKey(ts => ts.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TrackSubject>()
            .HasOne(ts => ts.Subject)
            .WithMany(s => s.TrackSubjects)
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureSubjectKnowledgeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectKnowledge>()
            .HasKey(sk => new { sk.SubjectId, sk.KnowledgeId });
            modelBuilder.Entity<SubjectKnowledge>()
            .HasOne(sk => sk.Subject)
            .WithMany(s => s.SubjectKnowledges)
            .HasForeignKey(sk => sk.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SubjectKnowledge>()
            .HasOne(sk => sk.Knowledge)
            .WithMany(k => k.SubjectKnowledges)
            .HasForeignKey(sk => sk.KnowledgeId)
            .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureKnowledgeTypeKnowledgeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeTypeKnowledge>()
            .HasKey(ktk => new { ktk.KnowledgeTypeId, ktk.KnowledgeId });
            modelBuilder.Entity<KnowledgeTypeKnowledge>()
            .HasOne(ktk => ktk.KnowledgeType)
            .WithMany(kt => kt.KnowledgeTypeKnowledges)
            .HasForeignKey(ktk => ktk.KnowledgeTypeId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<KnowledgeTypeKnowledge>()
            .HasOne(ktk => ktk.Knowledge)
            .WithMany(k => k.KnowledgeTypeKnowledges)
            .HasForeignKey(ktk => ktk.KnowledgeId)
            .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureKnowledgeTopicKnowledgeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeTopicKnowledge>()
            .HasKey(ktk => new { ktk.KnowledgeTopicId, ktk.KnowledgeId });
            modelBuilder.Entity<KnowledgeTopicKnowledge>()
            .HasOne(ktk => ktk.KnowledgeTopic)
            .WithMany(kt => kt.KnowledgeTopicKnowledges)
            .HasForeignKey(ktk => ktk.KnowledgeTopicId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<KnowledgeTopicKnowledge>()
            .HasOne(ktk => ktk.Knowledge)
            .WithMany(k => k.KnowledgeTopicKnowledges)
            .HasForeignKey(ktk => ktk.KnowledgeId)
            .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureLearningListEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningList>(entity =>
            {
                entity.HasKey(ll => ll.Id);
                entity.Property(ll => ll.Title).IsRequired();
                entity.HasOne(ll => ll.User)
                    .WithMany(u => u.LearningLists)
                    .HasForeignKey(ll => ll.LearnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureLearningListKnowledgeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningListKnowledge>(entity =>
            {
                entity.HasKey(llk => new { llk.LearningListId, llk.KnowledgeId });

                entity.HasOne(llk => llk.LearningList)
                    .WithMany(ll => ll.LearningListKnowledges)
                    .HasForeignKey(llk => llk.LearningListId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(llk => llk.Knowledge)
                    .WithMany(k => k.LearningListKnowledges)
                    .HasForeignKey(llk => llk.KnowledgeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        #endregion
    }
}