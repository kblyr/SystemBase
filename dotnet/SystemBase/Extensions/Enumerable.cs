namespace SystemBase;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> values)
    {
        return values.Aggregate(Enumerable.Empty<T>(), (result, current) => result.Concat(current));
    }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> values, Func<T, bool> predicate)
    {
        return values.Aggregate(Enumerable.Empty<T>(), (result, current) => result.Concat(current.Where(predicate)));
    }

    public static IEnumerable<TItem> FlattenFrom<TContainer, TItem>(this IEnumerable<TContainer> values, Func<TContainer, IEnumerable<TItem>> select)
    {
        return values.Select(value => select(value).AsEnumerable()).Flatten();
    }

    public static IEnumerable<TItem> FlattenFrom<TContainer, TItem>(this IEnumerable<TContainer> values, Func<TContainer, IEnumerable<TItem>> select, Func<TItem, bool> predicate)
    {
        return values.Select(value => select(value).AsEnumerable()).Flatten(predicate);
    }
}