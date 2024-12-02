namespace SystemBase;

public static class Pagination
{
    public record Query
    {
        public int Page { get; init; }
        public int Size { get; init; }   

        static readonly Query _default = new() { Page = 1, Size = 100 };
        public static Query Default => _default;
    }

    public record Result
    {
        public int CurrentPage { get; init; }
        public int PagesCount { get; init; }

        static readonly Result _default = new() { CurrentPage = 0, PagesCount = 0 };
        public static Result Default => _default;
    }
}