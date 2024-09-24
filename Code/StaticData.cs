namespace RandomDataGenerator.Code
{
    public static class StaticData
    {
        public static readonly IReadOnlyList<ListItem<Region>> _regions = [new("United States", Region.EN), new("Polska", Region.PL), new("Россия", Region.RU)];
    }

    public record ListItem<T>(string Name, T Value);
}
