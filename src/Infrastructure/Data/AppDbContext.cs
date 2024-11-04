using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Knowledge> Knowledges { get; set; }
        public DbSet<KnowledgeTopic> KnowledgeTopics { get; set; }
        public DbSet<KnowledgeType> KnowledgeTypes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackSubject> TrackSubjects { get; set; }
        public DbSet<SubjectKnowledge> SubjectKnowledges { get; set; }
        public DbSet<KnowledgeTypeKnowledge> KnowledgeTypeKnowledges { get; set; }
        public DbSet<KnowledgeTopicKnowledge> KnowledgeTopicKnowledges { get; set; }

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
            ConfigureManyToManyRelationships(modelBuilder);

            Seed(modelBuilder);
        }

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
                    .OnDelete(DeleteBehavior.Restrict);

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
                    .OnDelete(DeleteBehavior.Restrict);

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
                    .OnDelete(DeleteBehavior.Restrict);

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

        private void ConfigureManyToManyRelationships(ModelBuilder modelBuilder)
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
        }
    }
}