namespace SystemBase;

public static class StringUtilities
{
    public static string ToSnakeCase(string value)
    {
        var cleansed = new CleansedString(value);

        if (!cleansed.IsValid)
        {
            return "";
        }

        var builder = new System.Text.StringBuilder();

        for (var idx = 0; idx < cleansed.Value.Length; idx++)
        {
            if (idx == 0)
            {
                builder.Append(char.ToLower(cleansed.Value[idx]));
            }
            else if (char.IsUpper(cleansed.Value[idx]))
            {
                builder.Append($"_{char.ToLower(cleansed.Value[idx])}");
            }
        }

        return builder.ToString();
    }
}