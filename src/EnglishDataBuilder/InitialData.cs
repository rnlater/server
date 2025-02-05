using Domain.Entities.SingleIdEntities;
using Shared.Constants;

namespace EnglishDataBuilder
{
    public class InitialData
    {
        public static readonly Guid KnowledgeTopicId = Guid.Parse("8d083b77-4cef-4fad-a61a-2ebc88d974ee");
        public static readonly Guid TrackThreeId = Guid.Parse("8e3a7711-44e9-4cbb-bc8c-bc7be856cf88");
        public static readonly Guid TrackFiveId = Guid.Parse("58b14541-e330-474d-a215-f84950772f18");
        public static readonly Guid KnowledgeTypeId = Guid.Parse("605314ff-dc31-463b-af13-907020ef16c8");
        public static readonly Guid GameChooseId = Guid.Parse("9f16d50b-ec45-4873-95ba-13a8f8d9cf70");
        public static readonly Guid GameFillId = Guid.Parse("66d363b1-ef34-4b91-849a-aa13c814e73a");
        public static readonly List<Guid> KnowledgeTypeIds =
        [
            Guid.Parse("605314ff-dc31-463b-af13-907020ef16c8"), // Vocabulary root type
            Guid.Parse("30c747bc-fd5b-40ec-9e2b-4c038deb3447"),
            Guid.Parse("3bb10325-16cc-414e-b90f-252d3cbe9b0c"),
            Guid.Parse("4003565e-b4a1-493c-91b8-c5b76099d341"),
            Guid.Parse("6d8165ee-6aeb-45ab-b0ea-10fecb7c152d"),
            Guid.Parse("89c28552-f9ef-406d-8782-6d902a75b3d6"),
            Guid.Parse("a14d6c7d-6174-4129-8a32-e11b6e554ec8"),
            Guid.Parse("ab499387-2512-4f6a-b82b-e79ea527da94"),
            Guid.Parse("b07db885-2864-4741-9e0c-e3bcc00b167c"),
            Guid.Parse("c6355ffc-461c-413b-bf3a-f9efd0606108"),
            Guid.Parse("ebf26839-f8ee-44d0-aebd-30e6314939fe"),
            Guid.Parse("ec3d431b-873d-4ffe-a972-cd615b70d02b"),
            Guid.Parse("ef249e0e-4d5a-4ed1-91c1-f0174ec99deb"),
            Guid.Parse("f9b311c2-e1ad-485b-9c62-611e485f5139"),
        ];
        public static readonly List<KnowledgeType> knowledgeTypes =
        [
            new KnowledgeType { Id = KnowledgeTypeIds[0], Name = "Vocabulary" },
            new KnowledgeType { Id = KnowledgeTypeIds[1], Name = "Pronoun", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[2], Name = "Preposition", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[3], Name = "Modal verb", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[4], Name = "Interjection", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[5], Name = "Numeral", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[6], Name = "Proper noun", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[7], Name = "Adjective", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[8], Name = "Conjunction", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[9], Name = "Verb", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[10], Name = "Abbreviation", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[11], Name = "Adverb", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[12], Name = "Phrasal verb", ParentId = KnowledgeTypeIds[0] },
            new KnowledgeType { Id = KnowledgeTypeIds[13], Name = "Noun", ParentId = KnowledgeTypeIds[0] },
        ];

        public static readonly KnowledgeTopic knowledgeTopic = new KnowledgeTopic { Id = KnowledgeTopicId, Title = "English Vocabulary" };
        public static readonly Track trackThree = new Track
        {
            Id = TrackThreeId,
            Name = "3K Basic English Vocabularies",
            Description = "Basic English",
        };
        public static readonly Track trackFive = new Track
        {
            Id = TrackFiveId,
            Name = "5K Advance English Vocabularies",
            Description = "Advance English",
        };
        public static readonly Game gameChoose = new Game { Id = GameChooseId, Name = Games.ChooseTheCorrectAnswer, Description = "Choose the correct answer from four options", ImageUrl = "" };
        public static readonly Game gameFill = new Game { Id = GameFillId, Name = Games.FillInTheBlank, Description = "Fill in the blank with the correct word", ImageUrl = "" };
        public static readonly Game gameArrange = new Game { Id = Guid.NewGuid(), Name = Games.ArrangeWordsLetters, Description = "Arrange the words to form a correct sentence", ImageUrl = "" };
    }
}