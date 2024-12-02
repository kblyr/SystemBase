namespace SystemBase;

public readonly struct CleansedString
{
    public string Value { get; }
    public bool IsValid { get; }

    public CleansedString(string? value)
    {
        Value = value?.Trim() ?? "";
        IsValid = !string.IsNullOrWhiteSpace(Value);
    }

    public static implicit operator string(CleansedString value) => value.Value;
}