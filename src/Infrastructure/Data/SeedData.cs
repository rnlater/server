using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Shared.Utils;

namespace Infrastructure.Data;
public static class SeedData
{
    public static readonly Guid UserId = Guid.NewGuid();
    public static User[] GetUsers() => new[]
    {
        new User
        {
            Id = UserId,
            UserName = "testuser",
            Email = "testuser@example.com",
        }
    };

    public static Authentication[] GetAuthentications() => new[]
    {
        new Authentication
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            HashedPassword = PasswordHasher.HashWithSHA256("password"),
            IsEmailConfirmed = true,
            IsActivated = true
        }
    };

    public static readonly Guid Knowledge1Id = Guid.NewGuid();
    public static readonly Guid Knowledge2Id = Guid.NewGuid();
    public static Knowledge[] GetKnowledges() => new[]
{
        new Knowledge
        {
            Id = Knowledge1Id,
            Title = "Introduction to Algebra",
            Level = KnowledgeLevel.Beginner,
            Visibility = KnowledgeVisibility.Public,
            CreatorId = UserId
        },
        new Knowledge
        {
            Id = Knowledge2Id,
            Title = "Introduction to Physics",
            Level = KnowledgeLevel.Expert,
            Visibility = KnowledgeVisibility.Public,
            CreatorId = UserId
        }
    };

    public static readonly Guid Subject1Id = Guid.NewGuid();
    public static readonly Guid Subject2Id = Guid.NewGuid();
    public static Subject[] GetSubjects() => new[]
    {
        new Subject { Id = Subject1Id, Name = "Mathematics", Description = "Study of numbers, shapes, and patterns.", Photo = "test.png" },
        new Subject { Id = Subject2Id, Name = "Science", Description = "Study of the physical and natural world.", Photo = "test.png" }
    };

    public static SubjectKnowledge[] GetSubjectKnowledges() => new[]
    {
        new SubjectKnowledge { SubjectId = Subject1Id, KnowledgeId = Knowledge1Id },
        new SubjectKnowledge { SubjectId = Subject2Id, KnowledgeId = Knowledge2Id }
    };

    public static readonly Guid Track1Id = Guid.NewGuid();
    public static readonly Guid Track2Id = Guid.NewGuid();
    public static Track[] GetTracks() => new[]
{
        new Track { Id = Track1Id, Name = "Mathematics Track", Description = "A track focused on Mathematics." },
        new Track { Id = Track2Id, Name = "Science Track", Description = "A track focused on Science." }
    };

    public static TrackSubject[] GetTrackSubjects() => new[]
    {
        new TrackSubject { TrackId = Track1Id, SubjectId = Subject1Id },
        new TrackSubject { TrackId = Track2Id, SubjectId = Subject2Id }
    };

    public static readonly Guid KnowledgeType1Id = Guid.NewGuid();
    public static readonly Guid KnowledgeType2Id = Guid.NewGuid();
    public static KnowledgeType[] GetKnowledgeTypes() => new[]
{
        new KnowledgeType { Id = KnowledgeType1Id, Name = "Theory" },
        new KnowledgeType { Id = KnowledgeType2Id, Name = "Practical" }
    };
    public static KnowledgeTypeKnowledge[] GetKnowledgeTypeKnowledges() => new[]
    {
        new KnowledgeTypeKnowledge { KnowledgeTypeId = KnowledgeType1Id, KnowledgeId = Knowledge1Id },
        new KnowledgeTypeKnowledge { KnowledgeTypeId = KnowledgeType2Id, KnowledgeId = Knowledge2Id }
    };

    public static readonly Guid KnowledgeTopic1Id = Guid.NewGuid();
    public static readonly Guid KnowledgeTopic2Id = Guid.NewGuid();
    public static KnowledgeTopic[] GetKnowledgeTopics() => new[]
    {
        new KnowledgeTopic { Id = KnowledgeTopic1Id, Title = "Algebra", Order = 1 },
        new KnowledgeTopic { Id = KnowledgeTopic2Id, Title = "Physics", Order = 2 }
    };

    public static KnowledgeTopicKnowledge[] GetKnowledgeTopicKnowledges() => new[]
    {
        new KnowledgeTopicKnowledge { KnowledgeTopicId = KnowledgeTopic1Id, KnowledgeId = Knowledge1Id },
        new KnowledgeTopicKnowledge { KnowledgeTopicId = KnowledgeTopic2Id, KnowledgeId = Knowledge2Id }
    };

    public static readonly Guid Material1Id = Guid.NewGuid();
    public static readonly Guid Material2Id = Guid.NewGuid();
    public static Material[] GetMaterials() => new[]
    {
        new Material
        {
            Id = Material1Id,
            Type = MaterialType.Video,
            Content = "Video content about Algebra.",
            KnowledgeId = Knowledge1Id,
            Order = 1
        },
        new Material
        {
            Id = Material2Id,
            Type = MaterialType.TextMedium,
            Content = "Article about Physics.",
            KnowledgeId = Knowledge2Id,
            Order = 2
        }
    };

    public static readonly Guid Game1Id = Guid.NewGuid();
    public static readonly Guid Game2Id = Guid.NewGuid();
    public static Game[] GetGames()
    {
        return new[]
        {
                new Game { Id = Game1Id, Name = "Game 1", Description = "Description 1", ImageUrl = "image1.jpg" },
                new Game { Id = Game2Id, Name = "Game 2", Description = "Description 2", ImageUrl = "image2.jpg" }
            };
    }

    public static readonly Guid GameKnowledgeSubscription1Id = Guid.NewGuid();
    public static readonly Guid GameKnowledgeSubscription2Id = Guid.NewGuid();

    public static GameKnowledgeSubscription[] GetGameKnowledgeSubscriptions()
    {
        return new[]
        {
                new GameKnowledgeSubscription { Id = GameKnowledgeSubscription1Id, GameId = Game1Id, KnowledgeId = Knowledge1Id },
                new GameKnowledgeSubscription { Id = GameKnowledgeSubscription2Id, GameId = Game2Id, KnowledgeId = Knowledge2Id }
            };
    }

    public static readonly Guid GameOption1Id = Guid.NewGuid();
    public static readonly Guid GameOption2Id = Guid.NewGuid();
    public static readonly Guid GameOption3Id = Guid.NewGuid();
    public static readonly Guid GameOption4Id = Guid.NewGuid();
    public static readonly Guid GameOption5Id = Guid.NewGuid();
    public static readonly Guid GameOption6Id = Guid.NewGuid();
    public static GameOption[] GetGameOptions()
    {
        return new[]
        {
                new GameOption { Id = GameOption1Id, Type = GameOptionType.Question, Value = "What is Value 1?", GameKnowledgeSubscriptionId = GameKnowledgeSubscription1Id, Group = 1 },
                new GameOption { Id = GameOption2Id, Type = GameOptionType.Answer, Value = "Value 1", GameKnowledgeSubscriptionId = GameKnowledgeSubscription1Id, Group = 1, IsCorrect = true },
                new GameOption { Id = GameOption3Id, Type = GameOptionType.Answer, Value = "Wrong Value", GameKnowledgeSubscriptionId = GameKnowledgeSubscription1Id, Group = 1 },

                new GameOption { Id = GameOption4Id, Type = GameOptionType.Question, Value = "What is Value 2?", GameKnowledgeSubscriptionId = GameKnowledgeSubscription2Id, Group = 2 },
                new GameOption { Id = GameOption5Id, Type = GameOptionType.Answer, Value = "Value 2", GameKnowledgeSubscriptionId = GameKnowledgeSubscription2Id, Group = 2, IsCorrect = true },
                new GameOption { Id = GameOption6Id, Type = GameOptionType.Answer, Value = "Value 2", GameKnowledgeSubscriptionId = GameKnowledgeSubscription2Id, Group = 2},
            };
    }

    public static readonly Guid Learning1Id = Guid.NewGuid();
    public static readonly Guid Learning2Id = Guid.NewGuid();
    public static Learning[] GetLearnings()
    {
        return new[]
        {
                new Learning { Id = Learning1Id, KnowledgeId = Knowledge1Id, UserId = UserId },
                new Learning { Id = Learning2Id, KnowledgeId = Knowledge2Id, UserId = UserId }
            };
    }

    public static readonly Guid LearningHistory1Id = Guid.NewGuid();
    public static readonly Guid LearningHistory2Id = Guid.NewGuid();
    public static LearningHistory[] GetLearningHistories()
    {
        return new[]
        {
                new LearningHistory { Id = LearningHistory1Id, LearningId = Learning1Id, LearningLevel = LearningLevel.LevelZero, IsMemorized = true, Score = 100, PlayedGameId = Game1Id },
                new LearningHistory { Id = LearningHistory2Id, LearningId = Learning2Id, LearningLevel = LearningLevel.LevelOne, IsMemorized = false, Score = 80, PlayedGameId = Game2Id }
            };
    }
}
