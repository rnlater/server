using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using EnglishDataBuilder.Models;
using Infrastructure.Data;
using Shared.Constants;

namespace EnglishDataBuilder
{
    class VocabularyBuilder
    {
        public static List<JsonVocabulary> GetVocabularies(
            string dir
        )
        {
            List<JsonVocabulary> vocabularies = new List<JsonVocabulary>();
            var files = Directory.GetFiles(dir, "*.json");
            foreach (var file in files)
            {
                var fileVocabularies = Utils.ReadJsonFileAsync<List<JsonVocabulary>>(file).Result;
                if (fileVocabularies == null) continue;
                vocabularies.AddRange(fileVocabularies);

                var v2FilePath = dir.Replace("v1", "v2") + "/" + Path.GetFileName(file);
                if (File.Exists(v2FilePath))
                {
                    var v2Vocabularies = Utils.ReadJsonFileAsync<List<JsonVocabulary>>(v2FilePath).Result;
                    if (v2Vocabularies != null)
                    {
                        vocabularies.AddRange(v2Vocabularies);
                    }
                }
            }

            return vocabularies;
        }

        public static void Build(
            AppDbContext context,
            Dictionary<string, (Subject?, Subject?, KnowledgeTopic)> subjectBasicAdvancedTopics,
            List<JsonVocabulary> vocabularies,
            string dir
        )
        {
            GenerateKnowledges(context, vocabularies, subjectBasicAdvancedTopics, dir);
        }

        public static void GenerateKnowledges(
            AppDbContext context,
            List<JsonVocabulary> vocabularies,
            Dictionary<string, (Subject?, Subject?, KnowledgeTopic)> subjectBasicAdvancedTopics,
            string dir
        )
        {
            try
            {
                foreach (var vocabulary in vocabularies)
                {
                    var knowledge = new Knowledge
                    {
                        Title = vocabulary.Word,
                        Visibility = Domain.Enums.KnowledgeVisibility.Public,
                        Level = GetAvgLevel(vocabulary.Level),
                        CreatorId = GuidConstants.Admin,
                    };
                    context.Knowledges.Add(knowledge);

                    if (vocabulary.Photo != null)
                    {
                        context.Materials.Add(new Material
                        {
                            Type = Domain.Enums.MaterialType.Image,
                            Content = "Upload/Files/knowledges/images/" + vocabulary.Photo,
                            KnowledgeId = knowledge.Id,
                        });
                    }
                    if (vocabulary.Audio != null)
                    {
                        foreach (var item in vocabulary.Audio)
                        {
                            context.Materials.Add(new Material
                            {
                                Type = Domain.Enums.MaterialType.Audio,
                                Content = "Upload/Files/knowledges/audios/" + item,
                                KnowledgeId = knowledge.Id,
                            });
                        }
                    }
                    var MaterialOrder = 0;
                    var mergedMaterial = vocabulary.GetMergedMaterial();
                    foreach (var item in mergedMaterial.Phonetics)
                    {
                        context.Materials.Add(new Material
                        {
                            Type = Domain.Enums.MaterialType.TextMedium,
                            Content = item,
                            KnowledgeId = knowledge.Id,
                            Order = MaterialOrder++,
                        });
                    }
                    foreach (var item in mergedMaterial.Meanings)
                    {
                        var parent = new Material
                        {
                            Type = Domain.Enums.MaterialType.TextMedium,
                            Content = item.PartOfSpeech ?? "",
                            KnowledgeId = knowledge.Id,
                        };
                        context.Materials.Add(parent);
                        var MeaningOrder = 0;
                        foreach (var definition in item.Definitions)
                        {
                            context.Materials.Add(new Material
                            {
                                Type = Domain.Enums.MaterialType.Interpretation,
                                Content = definition.DefinitionText,
                                KnowledgeId = knowledge.Id,
                                ParentId = parent.Id,
                                Order = MeaningOrder++,
                            });
                            if (definition.Example != null)
                            {
                                context.Materials.Add(new Material
                                {
                                    Type = Domain.Enums.MaterialType.TextSmall,
                                    Content = definition.Example,
                                    KnowledgeId = knowledge.Id,
                                    ParentId = parent.Id,
                                    Order = MeaningOrder++,
                                });
                            }
                        }
                    }

                    foreach (var category in vocabulary.Categories)
                    {
                        if (dir.Contains("Basic"))
                        {
                            var subject = subjectBasicAdvancedTopics[category].Item1;
                            context.SubjectKnowledges.Add(new SubjectKnowledge
                            {
                                KnowledgeId = knowledge.Id,
                                SubjectId = subject!.Id,
                            });
                        }
                        else if (dir.Contains("Advanced"))
                        {
                            var subject = subjectBasicAdvancedTopics[category].Item2;
                            context.SubjectKnowledges.Add(new SubjectKnowledge
                            {
                                KnowledgeId = knowledge.Id,
                                SubjectId = subject!.Id,
                            });
                        }

                        var topic = subjectBasicAdvancedTopics[category].Item3;
                        context.KnowledgeTopicKnowledges.Add(new KnowledgeTopicKnowledge
                        {
                            KnowledgeId = knowledge.Id,
                            KnowledgeTopicId = topic.Id,
                        });
                    }

                    foreach (var item in vocabulary.GetListPartOfSpeech())
                    {
                        var typeName = Utils.ConvertToCamelCase(item);
                        var type = context.KnowledgeTypes.FirstOrDefault(x => x.Name == typeName);

                        context.KnowledgeTypeKnowledges.Add(new KnowledgeTypeKnowledge
                        {
                            KnowledgeId = knowledge.Id,
                            KnowledgeTypeId = type!.Id,
                        });
                    }

                    List<string> definitions = [vocabulary.GetADefinition()];
                    List<string> titles = [vocabulary.Word];

                    var random = new Random();
                    var randomVocabularies = vocabularies.OrderBy(x => random.Next()).Take(3).ToList();
                    foreach (var randomVocabulary in randomVocabularies)
                    {
                        string definition = randomVocabulary.GetADefinition();
                        string title = randomVocabulary.Word;
                        while (string.IsNullOrEmpty(definition))
                        {
                            var newRandomVocabulary = vocabularies.OrderBy(x => random.Next()).First();
                            definition = newRandomVocabulary.GetADefinition();
                            title = newRandomVocabulary.Word;
                        }
                        definitions.Add(definition);
                        titles.Add(title);
                    }

                    var ChooseTheCorrectAnswerGame = context.Games.First(x => x.Name == "Choose the correct answer");
                    var gameKnowledgeSubscription = new GameKnowledgeSubscription
                    {
                        GameId = ChooseTheCorrectAnswerGame.Id,
                        KnowledgeId = knowledge.Id,
                    };
                    context.GameKnowledgeSubscriptions.Add(gameKnowledgeSubscription);

                    var Group = 1;
                    context.GameOptions.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = vocabulary.Word,
                        Type = Domain.Enums.GameOptionType.Question,
                        Group = Group,
                    });
                    int Order = 0;
                    foreach (var definition in definitions)
                    {
                        context.GameOptions.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = definition,
                            Type = Domain.Enums.GameOptionType.Answer,
                            IsCorrect = definition == vocabulary.GetADefinition(),
                            Group = Group,
                            Order = Order++,
                        });
                    }
                    Group += 1;

                    context.GameOptions.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = vocabulary.GetADefinition(),
                        Type = Domain.Enums.GameOptionType.Question,
                        Group = Group,
                    });
                    Order = 0;
                    foreach (var title in titles)
                    {
                        context.GameOptions.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = title,
                            Type = Domain.Enums.GameOptionType.Answer,
                            IsCorrect = title == vocabulary.Word,
                            Group = Group,
                            Order = Order++,
                        });
                    }
                    Group += 1;

                    var FillInTheBlankGame = context.Games.First(x => x.Name == "Fill in the blank");
                    gameKnowledgeSubscription = new GameKnowledgeSubscription
                    {
                        GameId = FillInTheBlankGame.Id,
                        KnowledgeId = knowledge.Id,
                    };
                    context.GameKnowledgeSubscriptions.Add(gameKnowledgeSubscription);

                    context.GameOptions.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = GetBlankedVersion(vocabulary.Word),
                        Type = Domain.Enums.GameOptionType.Question,
                        Group = 1,
                    });
                    context.GameOptions.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = vocabulary.Word,
                        Type = Domain.Enums.GameOptionType.Answer,
                        IsCorrect = true,
                        Group = 1,
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetBlankedVersion(string input)
        {
            var words = input.Split(" ");
            if (words.Length == 1)
            {
                return ApplyUnderscore(words[0]);
            }
            else if (words.Length >= 2 && words.Length <= 3)
            {
                var random = new Random();
                var index = random.Next(words.Length);
                words[index] = ApplyUnderscore(words[index]);
                return string.Join(' ', words);
            }
            else if (words.Length > 3)
            {
                var random = new Random();
                var index = random.Next(words.Length);
                words[index] = new string('_', words[index].Length);
                return string.Join(' ', words);
            }

            return input;
        }

        private static string ApplyUnderscore(string word)
        {
            if (word.Length >= 1 && word.Length <= 4)
            {
                var random = new Random();
                var index = word.Length == 1 ? 0 : random.Next(1, word.Length - 1);
                var chars = word.ToCharArray();
                chars[index] = '_';
                var underscoredWord = new string(chars);

                return underscoredWord;
            }
            else
            {
                var random = new Random();
                var chars = word.ToCharArray();
                var index1 = random.Next(1, word.Length - 1);
                var index2 = random.Next(1, word.Length - 1);

                while (index2 == index1)
                {
                    index2 = random.Next(1, word.Length - 1);
                }

                chars[index1] = '_';
                chars[index2] = '_';
                var underscoredWord = new string(chars);

                return underscoredWord;
            }
        }
        private static Domain.Enums.KnowledgeLevel GetAvgLevel(List<string> levels)
        {
            if (levels == null || levels.Count == 0)
                return Domain.Enums.KnowledgeLevel.Beginner;

            var levelMapping = new Dictionary<string, int>
            {
                { "A1", 1 },
                { "A2", 2 },
                { "B1", 3 },
                { "B2", 4 },
                { "C1", 5 },
                { "C2", 6 }
            };

            var total = 0;
            foreach (var level in levels)
            {
                if (levelMapping.TryGetValue(level, out var value))
                {
                    total += value;
                }
            }

            var avg = (double)total / levels.Count;
            if (avg <= 1.5) return Domain.Enums.KnowledgeLevel.Beginner;
            if (avg <= 3.5) return Domain.Enums.KnowledgeLevel.Intermediate;
            return Domain.Enums.KnowledgeLevel.Expert;
        }
    }
}