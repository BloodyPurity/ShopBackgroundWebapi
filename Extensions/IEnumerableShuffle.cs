namespace ShopBackgroundSystem.Extensions
{
    public static class IEnumerableShuffle
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)//打乱
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

    }
}
