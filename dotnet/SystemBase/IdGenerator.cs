namespace SystemBase;

public interface IIdGenerator
{
    Guid Generate();
}

sealed class IdGenerator : IIdGenerator
{
    public Guid Generate()
    {
        return NewId.NextGuid();
    }
}