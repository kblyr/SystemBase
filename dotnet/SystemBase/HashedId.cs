using HashidsNet;

namespace SystemBase;

public interface IHashedId 
{
    byte ToByte(string hash);
    byte? ToNullableByte(string? hash);
    byte[] ToBytes(string hash);
    byte[] ToBytes(IEnumerable<string> hashes);
    short ToInt16(string hash);
    short? ToNullableInt16(string? hash);
    short[] ToInt16s(string hash);
    short[] ToInt16s(IEnumerable<string> hashes);
    int ToInt32(string hash);
    int? ToNullableInt32(string? hash);
    int[] ToInt32s(string hash);
    int[] ToInt32s(IEnumerable<string> hashes);
    long ToInt64(string hash);
    long? ToNullableInt64(string? hash);
    long[] ToInt64s(string hash);
    long[] ToInt64s(IEnumerable<string> hashes);
    Guid ToGuid(string hash);
    string FromByte(byte id);
    string? FromNullableByte(byte? id);
    string FromBytes(IEnumerable<byte> ids);
    IEnumerable<string> FromBytesToHashes(IEnumerable<byte> ids);
    string FromInt16(short id);
    string? FromNullableInt16(short? id);
    string FromInt16s(IEnumerable<short> ids);
    IEnumerable<string> FromInt16sToHashes(IEnumerable<short> ids);
    string FromInt32(int id);
    string? FromNullableInt32(int? id);
    string FromInt32s(IEnumerable<int> ids);
    IEnumerable<string> FromInt32sToHashes(IEnumerable<int> ids);
    string FromInt64(long id);
    string? FromNullableInt64(long? id);
    string FromInt64s(IEnumerable<long> ids);
    IEnumerable<string> FromInt64sToHashes(IEnumerable<long> ids);
    string FromGuid(Guid id);
    string? FromNullableGuid(Guid? id);
}

sealed class HashedId(string salt, int minLength) : IHashedId
{
    readonly Hashids _hashids = new(salt, minLength);

    public byte ToByte(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return 0;
        }

        try
        {
            return Convert.ToByte(_hashids.DecodeSingle(hash));
        }
        catch
        {
            return 0;
        }
    }

    public byte? ToNullableByte(string? hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return null;
        }

        try
        {
            return Convert.ToByte(_hashids.DecodeSingle(hash));
        }
        catch
        {
            return null;
        }
    }

    public byte[] ToBytes(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return [];
        }

        try
        {
            return _hashids.Decode(hash).Select(Convert.ToByte).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public byte[] ToBytes(IEnumerable<string> hashes)
    {
        if (hashes is null || !hashes.Any())
        {
            return [];
        }

        try
        {
            return hashes.Select(ToByte).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public short ToInt16(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return 0;
        }

        try
        {
            return Convert.ToInt16(_hashids.DecodeSingle(hash));
        }
        catch
        {
            return 0;
        }
    }

    public short? ToNullableInt16(string? hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return null;
        }

        try
        {
            return Convert.ToInt16(_hashids.DecodeSingle(hash));
        }
        catch
        {
            return null;
        }
    }

    public short[] ToInt16s(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return [];
        }

        try
        {
            return _hashids.Decode(hash).Select(Convert.ToInt16).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public short[] ToInt16s(IEnumerable<string> hashes)
    {
        if (hashes is null || !hashes.Any())
        {
            return [];
        }

        try
        {
            return hashes.Select(ToInt16).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public int ToInt32(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return 0;
        }

        try 
        {
            return _hashids.DecodeSingle(hash);
        }
        catch
        {
            return 0;
        }
    }

    public int? ToNullableInt32(string? hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return null;
        }

        try 
        {
            return _hashids.DecodeSingle(hash);
        }
        catch
        {
            return null;
        }
    }

    public int[] ToInt32s(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return [];
        }

        try
        {
            return _hashids.Decode(hash);
        }
        catch
        {
            return [];
        }
    }

    public int[] ToInt32s(IEnumerable<string> hashes)
    {
        if (hashes is null || !hashes.Any())
        {
            return [];
        }

        try
        {
            return hashes.Select(ToInt32).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public long ToInt64(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return 0;
        }

        try
        {
            return _hashids.DecodeSingleLong(hash);
        }
        catch 
        {
            return 0;
        }
    }

    public long? ToNullableInt64(string? hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return null;
        }

        try
        {
            return _hashids.DecodeSingleLong(hash);
        }
        catch 
        {
            return null;
        }
    }

    public long[] ToInt64s(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return [];
        }

        try
        {
            return _hashids.DecodeLong(hash);
        }
        catch
        {
            return [];
        }
    }

    public long[] ToInt64s(IEnumerable<string> hashes)
    {
        if (hashes is null || !hashes.Any())
        {
            return [];
        }

        try
        {
            return hashes.Select(ToInt64).ToArray();
        }
        catch
        {
            return [];
        }
    }

    public Guid ToGuid(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return Guid.Empty;
        }

        try
        {
            return new Guid(_hashids.Decode(hash).Select(Convert.ToByte).ToArray());
        }
        catch
        {
            return Guid.Empty;
        }
    }

    public string FromByte(byte id)
    {
        try
        {
            return _hashids.Encode(id);
        }
        catch
        {
            return "";
        }
    }

    public string? FromNullableByte(byte? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        return FromByte(id.Value);
    }

    public string FromBytes(IEnumerable<byte> ids)
    {
        if (ids is null || !ids.Any())
        {
            return "";
        }

        try
        {
            return _hashids.Encode(ids.Select(Convert.ToInt32));
        }
        catch
        {
            return "";
        }
    }

    public IEnumerable<string> FromBytesToHashes(IEnumerable<byte> ids)
    {
        if (ids is null || !ids.Any())
        {
            return [];
        }

        try
        {
            return ids.Select(FromByte);
        }
        catch
        {
            return [];
        }
    }

    public string FromInt16(short id)
    {
        try
        {
            return _hashids.Encode(id);
        }
        catch
        {
            return "";
        }
    }

    public string? FromNullableInt16(short? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        return FromInt16(id.Value);
    }

    public string FromInt16s(IEnumerable<short> ids)
    {
        if (ids is null || !ids.Any())
        {
            return "";
        }

        try
        {
            return _hashids.Encode(ids.Select(Convert.ToInt32));
        }
        catch
        {
            return "";
        }
    }

    public IEnumerable<string> FromInt16sToHashes(IEnumerable<short> ids)
    {
        if (ids is null || !ids.Any())
        {
            return [];
        }

        try
        {
            return ids.Select(FromInt16);
        }
        catch
        {
            return [];
        }
    }

    public string FromInt32(int id)
    {
        try
        {
            return _hashids.Encode(id);
        }
        catch
        {
            return "";
        }
    }

    public string? FromNullableInt32(int? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        return FromInt32(id.Value);
    }

    public string FromInt32s(IEnumerable<int> ids)
    {
        if (ids is null || !ids.Any())
        {
            return "";
        }

        try
        {
            return _hashids.Encode(ids);
        }
        catch
        {
            return "";
        }
    }

    public IEnumerable<string> FromInt32sToHashes(IEnumerable<int> ids)
    {
        if (ids is null || !ids.Any())
        {
            return [];
        }

        try
        {
            return ids.Select(FromInt32);
        }
        catch
        {
            return [];
        }
    }

    public string FromInt64(long id)
    {
        try
        {
            return _hashids.EncodeLong(id);
        }
        catch
        {
            return "";
        }
    }

    public string? FromNullableInt64(long? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        return FromInt64(id.Value);
    }

    public string FromInt64s(IEnumerable<long> ids)
    {
        if (ids is null || !ids.Any())
        {
            return "";
        }

        try
        {
            return _hashids.EncodeLong(ids);
        }
        catch
        {
            return "";
        }
    }

    public IEnumerable<string> FromInt64sToHashes(IEnumerable<long> ids)
    {
        if (ids is null || !ids.Any())
        {
            return [];
        }

        try
        {
            return ids.Select(FromInt64);
        }
        catch
        {
            return [];
        }
    }
    
    public string FromGuid(Guid id)
    {
        if (id == Guid.Empty)
        {
            return "";
        }

        try
        {
            return _hashids.Encode(id.ToByteArray().Select(Convert.ToInt32));
        }
        catch
        {
            return "";
        }
    }

    public string? FromNullableGuid(Guid? id)
    {
        if (!id.HasValue)
        {
            return null;
        }

        return FromGuid(id.Value);
    }
}

public interface IHashedIdFactory
{
    IHashedId Create(string salt, int minLength);
    IHashedId<byte> Byte(string salt, int minLength);
    IHashedId<short> Int16(string salt, int minLength);
    IHashedId<int> Int32(string salt, int minLength);
    IHashedId<long> Int64(string salt, int minLength);
    IHashedId<Guid> Guid(string salt, int minLength);
}

sealed class HashedIdFactory : IHashedIdFactory
{
    public IHashedId Create(string salt, int minLength) => new HashedId(salt, minLength);

    public IHashedId<byte> Byte(string salt, int minLength) => new HashedIdByte(new Hashids(salt, minLength));

    public IHashedId<short> Int16(string salt, int minLength) => new HashedIdInt16(new Hashids(salt, minLength));

    public IHashedId<int> Int32(string salt, int minLength) => new HashedIdInt32(new Hashids(salt, minLength));

    public IHashedId<long> Int64(string salt, int minLength) => new HashedIdInt64(new Hashids(salt, minLength));

    public IHashedId<Guid> Guid(string salt, int minLength) => new HashedIdGuid(new Hashids(salt, minLength));
}

public interface IHashedId<T> where T : notnull
{
    string ToHash(T id);
    string[] ToHashes(T[] ids);
    T FromHash(string hashedId);
    T[] FromHashes(string[] hashedIds);
    string? ToNullableHash(T? id);
    string[]? ToNullableHashes(T[]? id);
    T? FromNullableHash(string? hashedId);
    T[]? FromNullableHashes(string[]? hashedIds);
}

public abstract class HashedIdBase<T> where T : notnull
{
    protected abstract T Decode(string hashedId);

    protected abstract string Encode(T id);

    public virtual string ToHash(T id) => EqualityComparer<T>.Default.Equals(id, default) ? "" : Encode(id);

    public virtual string[] ToHashes(T[] ids) => ids.Length == 0 ? [] : [..ids.Select(ToHash)];

    public virtual T FromHash(string hashedId) => string.IsNullOrWhiteSpace(hashedId) ? default! : Decode(hashedId);

    public virtual T[] FromHashes(string[] hashedIds) => [..hashedIds.Select(FromHash)];

    public virtual string? ToNullableHash(T? id) => id is null ? null : ToHash(id);

    public virtual string[]? ToNullableHashes(T[]? ids) => ids is null ? null : ToHashes(ids);

    public virtual T? FromNullableHash(string? hashedId) => hashedId is null ? default : FromHash(hashedId);

    public virtual T[]? FromNullableHashes(string[]? hashedIds) => hashedIds is null ? null : FromHashes(hashedIds);
}

sealed class HashedIdByte(Hashids ids) : HashedIdBase<byte>, IHashedId<byte>
{
    protected override byte Decode(string hashedId) => Convert.ToByte(ids.DecodeSingle(hashedId));

    protected override string Encode(byte id) => ids.Encode(id);
}

sealed class HashedIdInt16(Hashids ids) : HashedIdBase<short>, IHashedId<short>
{
    protected override short Decode(string hashedId) => Convert.ToInt16(ids.DecodeSingle(hashedId));

    protected override string Encode(short id) => ids.Encode(id);
}

sealed class HashedIdInt32(Hashids ids) : HashedIdBase<int>, IHashedId<int>
{
    protected override int Decode(string hashedId) => ids.DecodeSingle(hashedId);

    protected override string Encode(int id) => ids.Encode(id);
}

sealed class HashedIdInt64(Hashids ids) : HashedIdBase<long>, IHashedId<long>
{
    protected override long Decode(string hashedId) => ids.DecodeSingleLong(hashedId);

    protected override string Encode(long id) => ids.EncodeLong(id);
}

sealed class HashedIdGuid(Hashids ids) : HashedIdBase<Guid>, IHashedId<Guid>
{
    protected override Guid Decode(string hashedId) => new(ids.Decode(hashedId).Select(Convert.ToByte).ToArray());

    protected override string Encode(Guid id) => ids.Encode(id.ToByteArray().Select(Convert.ToInt32));
}