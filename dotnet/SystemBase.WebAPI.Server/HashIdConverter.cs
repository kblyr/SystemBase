using Mapster;

namespace SystemBase;

public static class HashIdConverterInstance
{
    public static IHashIdConverter Instance => MapContext.Current.GetService<IHashIdConverter>();
}