namespace Shared.Constants
{
    public static class NeededReviewTime
    {
        public static readonly TimeSpan Level0 = TimeSpan.FromHours(1); // 1 hour
        public static readonly TimeSpan Level1 = TimeSpan.FromDays(1); // 3 days
        public static readonly TimeSpan Level2 = TimeSpan.FromDays(5); // 1 week
        public static readonly TimeSpan Level3 = TimeSpan.FromDays(14); // 2 weeks
        public static readonly TimeSpan Level4 = TimeSpan.FromDays(30); // 1 month
        public static readonly TimeSpan Level5 = TimeSpan.FromDays(90); // 3 months
        public static readonly TimeSpan NotMemorized = TimeSpan.FromHours(1); // 1 hour

    }
}