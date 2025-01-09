namespace Shared.Constants
{
    public static class RedisCache
    {
        public class Keys
        {
            public const string GetDetailedTracks = "GetDetailedTracks";
            public const string GetTrackByGuid = "GetTrackByGuid";
            public const string GetDetailedKnowledgeByGuid = "GetDetailedKnowledgeByGuid";
            public const string GetKnowledgeTypes = "GetKnowledgeTypes";
            public const string GetSubjectByGuid = "GetSubjectByGuid";
            public const string GetKnowledgeTopics = "GetKnowledgeTopics";
            public const string GetAllGames = "GetAllGames";
        }

        public static TimeSpan DefaultCacheExpiry = TimeSpan.FromHours(5);
        public static TimeSpan MostUsedCacheExpiry = TimeSpan.FromHours(20);
    }
}