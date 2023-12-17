using System.Text;

namespace SystemBase;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendIf(this StringBuilder builder, string value, bool condition)
    {
        return condition ? builder.Append(value) : builder;
    }

    public static StringBuilder AppendIf(this StringBuilder builder, char value, bool condition)
    {
        return condition ? builder.Append(value) : builder;
    }

    public static StringBuilder AppendCleansed(this StringBuilder builder, CleansedString value)
    {
        return builder.AppendIf(value, value.IsValid);
    }

    public static StringBuilder AppendCleansedThenIf(this StringBuilder builder, CleansedString value, string thenValue, bool condition)
    {
        return builder.AppendCleansed(value).AppendIf(thenValue, value.IsValid && condition);
    }

    public static StringBuilder AppendCleansedThenIf(this StringBuilder builder, CleansedString value, char thenValue, bool condition)
    {
        return builder.AppendCleansed(value).AppendIf(thenValue, value.IsValid && condition);
    }
}