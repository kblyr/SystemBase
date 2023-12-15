namespace SystemBase;

public interface IHashIdConverter 
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