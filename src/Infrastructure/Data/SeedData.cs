using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Shared.Utils;

namespace Infrastructure.Data;
public static class SeedData
{
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid Subject1Id = Guid.NewGuid();
    private static readonly Guid Subject2Id = Guid.NewGuid();
    private static readonly Guid Track1Id = Guid.NewGuid();
    private static readonly Guid Track2Id = Guid.NewGuid();
    private static readonly Guid KnowledgeType1Id = Guid.NewGuid();
    private static readonly Guid KnowledgeType2Id = Guid.NewGuid();
    private static readonly Guid KnowledgeTopic1Id = Guid.NewGuid();
    private static readonly Guid KnowledgeTopic2Id = Guid.NewGuid();
    private static readonly Guid Knowledge1Id = Guid.NewGuid();
    private static readonly Guid Knowledge2Id = Guid.NewGuid();

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

    public static Subject[] GetSubjects() => new[]
    {
        new Subject { Id = Subject1Id, Name = "Mathematics", Description = "Study of numbers, shapes, and patterns.", Photo = "test.png" },
        new Subject { Id = Subject2Id, Name = "Science", Description = "Study of the physical and natural world.", Photo = "test.png" }
    };

    public static Track[] GetTracks() => new[]
    {
        new Track { Id = Track1Id, Name = "Mathematics Track", Description = "A track focused on Mathematics." },
        new Track { Id = Track2Id, Name = "Science Track", Description = "A track focused on Science." }
    };

    public static KnowledgeType[] GetKnowledgeTypes() => new[]
    {
        new KnowledgeType { Id = KnowledgeType1Id, Name = "Theory" },
        new KnowledgeType { Id = KnowledgeType2Id, Name = "Practical" }
    };

    public static KnowledgeTopic[] GetKnowledgeTopics() => new[]
    {
        new KnowledgeTopic { Id = KnowledgeTopic1Id, Title = "Algebra", Order = 1 },
        new KnowledgeTopic { Id = KnowledgeTopic2Id, Title = "Physics", Order = 2 }
    };

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
            Level = KnowledgeLevel.Beginner,
            Visibility = KnowledgeVisibility.Public,
            CreatorId = UserId
        }
    };

    public static Material[] GetMaterials() => new[]
    {
        new Material
        {
            Id = Guid.NewGuid(),
            Type = MaterialType.Video,
            Content = "Video content about Algebra.",
            KnowledgeId = Knowledge1Id,
            Order = 1
        },
        new Material
        {
            Id = Guid.NewGuid(),
            Type = MaterialType.Text,
            Content = "Article about Physics.",
            KnowledgeId = Knowledge2Id,
            Order = 2
        }
    };

    public static TrackSubject[] GetTrackSubjects() => new[]
    {
        new TrackSubject { TrackId = Track1Id, SubjectId = Subject1Id },
        new TrackSubject { TrackId = Track2Id, SubjectId = Subject2Id }
    };

    public static SubjectKnowledge[] GetSubjectKnowledges() => new[]
    {
        new SubjectKnowledge { SubjectId = Subject1Id, KnowledgeId = Knowledge1Id },
        new SubjectKnowledge { SubjectId = Subject2Id, KnowledgeId = Knowledge2Id }
    };

    public static KnowledgeTypeKnowledge[] GetKnowledgeTypeKnowledges() => new[]
    {
        new KnowledgeTypeKnowledge { KnowledgeTypeId = KnowledgeType1Id, KnowledgeId = Knowledge1Id },
        new KnowledgeTypeKnowledge { KnowledgeTypeId = KnowledgeType2Id, KnowledgeId = Knowledge2Id }
    };

    public static KnowledgeTopicKnowledge[] GetKnowledgeTopicKnowledges() => new[]
    {
        new KnowledgeTopicKnowledge { KnowledgeTopicId = KnowledgeTopic1Id, KnowledgeId = Knowledge1Id },
        new KnowledgeTopicKnowledge { KnowledgeTopicId = KnowledgeTopic2Id, KnowledgeId = Knowledge2Id }
    };
}


