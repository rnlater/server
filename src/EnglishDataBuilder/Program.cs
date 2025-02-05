using Domain.Entities.SingleIdEntities;
using EnglishDataBuilder;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

var host = Utils.CreateHostBuilder(args).Build();
var currentDir = Directory.GetCurrentDirectory().Replace("/bin/Debug/net8.0", "");
var JsonDirV1 = currentDir + "/data/v1/";
var three = "basic";
var five = "advanced";
var other = "other";
var categoriesFilename = currentDir + "/data/categories.json";

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();


    if (!context.KnowledgeTopics.Any(kt => kt.Id == InitialData.knowledgeTopic.Id))
    {
        await context.KnowledgeTopics.AddAsync(InitialData.knowledgeTopic);
    }
    if (!context.Tracks.Any(t => t.Id == InitialData.trackThree.Id))
    {
        await context.Tracks.AddAsync(InitialData.trackThree);
    }
    if (!context.Tracks.Any(t => t.Id == InitialData.trackFive.Id))
    {
        await context.Tracks.AddAsync(InitialData.trackFive);
    }
    if (!context.Games.Any(g => g.Id == InitialData.gameChoose.Id))
    {
        await context.Games.AddAsync(InitialData.gameChoose);
    }
    if (!context.Games.Any(g => g.Id == InitialData.gameFill.Id))
    {
        await context.Games.AddAsync(InitialData.gameFill);
    }
    if (!context.Games.Any(g => g.Id == InitialData.gameArrange.Id))
    {
        await context.Games.AddAsync(InitialData.gameArrange);
    }

    foreach (var item in InitialData.knowledgeTypes)
    {
        if (!context.KnowledgeTypes.Any(kt => kt.Id == item.Id))
        {
            await context.KnowledgeTypes.AddAsync(item);
        }
    }
    await context.SaveChangesAsync();

    try
    {
        Dictionary<string, (Subject?, Subject?, KnowledgeTopic)> subjectBasicAdvancedTopics = [];

        var threeKVocabs = VocabularyBuilder.GetVocabularies(JsonDirV1 + three);
        var fiveKVocabs = VocabularyBuilder.GetVocabularies(JsonDirV1 + five);
        var otherVocabs = VocabularyBuilder.GetVocabularies(JsonDirV1 + other);

        HashSet<string> categoriesBasic = [.. threeKVocabs.SelectMany(v => v.Categories)];
        HashSet<string> categoriesAdvanced = [.. fiveKVocabs.SelectMany(v => v.Categories)];

        await BuildCategory.BuildAsync(context, subjectBasicAdvancedTopics, categoriesBasic, categoriesAdvanced, categoriesFilename);

        VocabularyBuilder.Build(context, subjectBasicAdvancedTopics, fiveKVocabs, JsonDirV1 + five);
        VocabularyBuilder.Build(context, subjectBasicAdvancedTopics, threeKVocabs, JsonDirV1 + three);
        VocabularyBuilder.Build(context, subjectBasicAdvancedTopics, otherVocabs, JsonDirV1 + other);

        // TODO: track subject
        var subjectsBasic = categoriesBasic.Select(c => subjectBasicAdvancedTopics[c].Item1)
            .Where(s => s != null)
            .Select(s => s!);
        var subjectsAdvanced = categoriesAdvanced.Select(c => subjectBasicAdvancedTopics[c].Item2)
            .Where(s => s != null)
            .Select(s => s!);

        await BuildCategory.CreateTrackSubject(context, subjectsBasic, context.Tracks.FirstOrDefault(t => t.Id == InitialData.trackThree.Id)!);
        await BuildCategory.CreateTrackSubject(context, subjectsAdvanced, context.Tracks.FirstOrDefault(t => t.Id == InitialData.trackFive.Id)!);

        await context.SaveChangesAsync();
    }
    catch (Exception)
    {
        await context.Database.RollbackTransactionAsync();
        throw;
    }
}
