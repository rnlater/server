using System.Text.Json.Serialization;

namespace EnglishDataBuilder.Models
{
    public class JsonCategory
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("children")]
        public List<JsonCategory> Children { get; set; } = [];
    }

    public class JsonVocabulary
    {
        [JsonPropertyName("audio")]
        public List<string>? Audio { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = [];

        [JsonPropertyName("level")]
        public List<string> Level { get; set; } = [];

        [JsonPropertyName("materials")]
        public List<JsonMaterial> Materials { get; set; } = [];

        [JsonPropertyName("photo")]
        public string? Photo { get; set; }

        [JsonPropertyName("word")]
        public required string Word { get; set; }

        public List<string> GetListPartOfSpeech()
        {
            return Materials.SelectMany(m => m.Meanings)
                            .Select(static m => m.PartOfSpeech)
                            .Where(partOfSpeech => partOfSpeech != null)
                            .Distinct()
                            .ToList()!;
        }

        public string GetADefinition()
        {
            return Materials.SelectMany(m => m.Meanings)
                            .SelectMany(m => m.Definitions)
                            .Select(static d => d.DefinitionText)
                            .FirstOrDefault() ?? "";
        }

        public JsonMaterial GetMergedMaterial()
        {
            var mergedMaterial = new JsonMaterial
            {
                Phonetics = Materials.SelectMany(m => m.Phonetics).Distinct().ToList(),
                Meanings = MergeMeanings(Materials.SelectMany(m => m.Meanings).ToList())
            };

            return mergedMaterial;
        }

        static private List<JsonMeaning> MergeMeanings(List<JsonMeaning> meanings)
        {
            var mergedMeanings = new List<JsonMeaning>();

            foreach (var meaning in meanings)
            {
                var existingMeaning = mergedMeanings.FirstOrDefault(m => m.PartOfSpeech == meaning.PartOfSpeech);

                if (existingMeaning == null)
                {
                    mergedMeanings.Add(new JsonMeaning
                    {
                        PartOfSpeech = meaning.PartOfSpeech,
                        Definitions = meaning.Definitions,
                        Synonyms = meaning.Synonyms.Distinct().ToList(),
                        Antonyms = meaning.Antonyms.Distinct().ToList()
                    });
                }
                else
                {
                    existingMeaning.Definitions = existingMeaning.Definitions
                        .Concat(meaning.Definitions)
                        .Distinct(new DefinitionComparer())
                        .ToList();

                    existingMeaning.Synonyms = existingMeaning.Synonyms
                        .Concat(meaning.Synonyms)
                        .Distinct()
                        .ToList();

                    existingMeaning.Antonyms = existingMeaning.Antonyms
                        .Concat(meaning.Antonyms)
                        .Distinct()
                        .ToList();
                }
            }

            return mergedMeanings;
        }
    }

    public class JsonMaterial
    {
        [JsonPropertyName("meanings")]
        public List<JsonMeaning> Meanings { get; set; } = [];

        [JsonPropertyName("phonetics")]
        public List<string> Phonetics { get; set; } = [];

        public List<string> GetListAntonyms()
        {
            return Meanings.SelectMany(m => m.Antonyms)
                           .Distinct()
                           .ToList();
        }

        public List<string> GetListSynonyms()
        {
            return Meanings.SelectMany(m => m.Synonyms)
                           .Distinct()
                           .ToList();
        }
    }

    public class JsonMeaning
    {
        [JsonPropertyName("antonyms")]
        public List<string> Antonyms { get; set; } = [];

        [JsonPropertyName("definitions")]
        public List<JsonDefinition> Definitions { get; set; } = [];

        [JsonPropertyName("part_of_speech")]
        public string? PartOfSpeech { get; set; }

        [JsonPropertyName("synonyms")]
        public List<string> Synonyms { get; set; } = [];
    }

    public class JsonDefinition
    {
        [JsonPropertyName("definition")]
        public required string DefinitionText { get; set; }

        [JsonPropertyName("example")]
        public string? Example { get; set; }
    }

    public class DefinitionComparer : IEqualityComparer<JsonDefinition>
    {
        public bool Equals(JsonDefinition? x, JsonDefinition? y)
        {
            if (x == null || y == null)
                return false;

            return x.DefinitionText == y.DefinitionText && x.Example == y.Example;
        }

        public int GetHashCode(JsonDefinition obj)
        {
            if (obj == null)
                return 0;

            int hashDefinitionText = obj.DefinitionText == null ? 0 : obj.DefinitionText.GetHashCode();
            int hashExample = obj.Example == null ? 0 : obj.Example.GetHashCode();

            return hashDefinitionText ^ hashExample;
        }
    }
}