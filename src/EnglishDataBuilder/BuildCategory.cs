using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using EnglishDataBuilder.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnglishDataBuilder
{
    class BuildCategory
    {
        public static async Task BuildAsync(AppDbContext context, Dictionary<string, (Subject?, Subject?, KnowledgeTopic)> subjectBasicAdvancedTopics, HashSet<string> categoriesBasic, HashSet<string> categoriesAdvanced, string filePath)
        {
            try
            {
                var rootCategory = await context.KnowledgeTopics.FirstOrDefaultAsync(x => x.Title == "English Vocabulary");

                var categories = await Utils.ReadJsonFileAsync<List<JsonCategory>>(filePath);
                if (categories == null) return;

                for (int i = 0; i < categories.Count; i++)
                {
                    var topic = MaptoTopic(categories[i]);
                    topic.ParentId = rootCategory!.Id;
                    await context.KnowledgeTopics.AddAsync(topic);

                    for (int j = 0; j < categories[i].Children.Count; j++)
                    {
                        var child = categories[i].Children[j];

                        var childTopic = MaptoTopic(child);
                        childTopic.ParentId = topic.Id;
                        await context.KnowledgeTopics.AddAsync(childTopic);

                        for (int k = 0; k < child.Children.Count; k++)
                        {
                            var grandChild = child.Children[k];

                            if (subjectBasicAdvancedTopics.ContainsKey(grandChild.Title))
                            {
                                continue;
                            }

                            var grandChildTopic = MaptoTopic(grandChild); grandChildTopic.ParentId = childTopic.Id;
                            await context.KnowledgeTopics.AddAsync(grandChildTopic);


                            Subject? grandChildSubjectBasic = null;
                            Subject? grandChildSubjectAdvanced = null;

                            if (categoriesBasic.Contains(grandChild.Title))
                            {
                                grandChildSubjectBasic = MaptoSubject(grandChild, "Basic");
                                await context.Subjects.AddAsync(grandChildSubjectBasic);
                            }
                            if (categoriesAdvanced.Contains(grandChild.Title))
                            {
                                grandChildSubjectAdvanced = MaptoSubject(grandChild, "Advance");
                                await context.Subjects.AddAsync(grandChildSubjectAdvanced);
                            }

                            if (grandChild.Title == "skin")
                            {
                                Console.WriteLine("skin");
                            }

                            subjectBasicAdvancedTopics.TryAdd(grandChild.Title, (grandChildSubjectBasic, grandChildSubjectAdvanced, grandChildTopic));
                        }

                        if (subjectBasicAdvancedTopics.ContainsKey(child.Title))
                        {
                            continue;
                        }


                        Subject? childSubjectBasic = null;
                        Subject? childSubjectAdvanced = null;

                        if (categoriesBasic.Contains(child.Title))
                        {
                            childSubjectBasic = MaptoSubject(child, "Basic");
                            await context.Subjects.AddAsync(childSubjectBasic);
                        }
                        if (categoriesAdvanced.Contains(child.Title))
                        {
                            childSubjectAdvanced = MaptoSubject(child, "Advance");
                            await context.Subjects.AddAsync(childSubjectAdvanced);

                        }
                        subjectBasicAdvancedTopics.TryAdd(child.Title, (childSubjectBasic, childSubjectAdvanced, childTopic));


                    }

                    if (subjectBasicAdvancedTopics.ContainsKey(categories[i].Title))
                    {
                        continue;
                    }

                    Subject? subjectBasic = null;
                    Subject? subjectAdvanced = null;

                    if (categoriesBasic.Contains(categories[i].Title))
                    {
                        subjectBasic = MaptoSubject(categories[i], "Basic");
                        await context.Subjects.AddAsync(subjectBasic);
                    }
                    if (categoriesAdvanced.Contains(categories[i].Title))
                    {
                        subjectAdvanced = MaptoSubject(categories[i], "Advance");
                        await context.Subjects.AddAsync(subjectAdvanced);

                    }
                    subjectBasicAdvancedTopics.TryAdd(categories[i].Title, (subjectBasic, subjectAdvanced, topic));


                }

                if (categories.Count() != subjectBasicAdvancedTopics.Count())
                {
                    Console.WriteLine("Not all categories are added");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        static KnowledgeTopic MaptoTopic(JsonCategory category)
        {
            return new KnowledgeTopic
            {
                Title = Utils.ConvertToCamelCase(category.Title),
            };
        }

        static Subject MaptoSubject(JsonCategory category, string postfix)
        {
            return new Subject
            {
                Name = Utils.ConvertToCamelCase(category.Title) + " (" + postfix + ")",
                Description = "", // TODO: Add description
                Photo = "", // TODO: Add photo
            };
        }

        public static async Task CreateTrackSubject(AppDbContext context, IEnumerable<Subject> subjects, Track track)
        {
            try
            {
                foreach (var subject in subjects)
                {
                    var trackSubject = new TrackSubject
                    {
                        TrackId = track.Id,
                        SubjectId = subject.Id,
                    };

                    await context.TrackSubjects.AddAsync(trackSubject);
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }
}