using Google.Protobuf.WellKnownTypes;

namespace ProductService.Converter
{
    public static class ProtoConverter
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Convert DateTime to protobuf-compatible ticks
        public static long DateTimeToProtoTicks(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - UnixEpoch).Ticks;
        }

        // Convert protobuf ticks back to DateTime
        public static DateTime ProtoTicksToDateTime(long protoTicks)
        {
            return UnixEpoch.AddTicks(protoTicks).ToLocalTime(); // or .ToUniversalTime()
        }

        // Alternative: Convert to Google's Timestamp
        public static Timestamp DateTimeToProtoTimestamp(DateTime dateTime)
        {
            return Timestamp.FromDateTime(dateTime.ToUniversalTime());
        }

        // Convert from Google's Timestamp
        public static DateTime ProtoTimestampToDateTime(Timestamp timestamp)
        {
            return timestamp.ToDateTime().ToLocalTime(); // or .ToUniversalTime()
        }

        // Convert GUID to Proto string
        public static string GuidToProtoString(Guid guid)
        {
            return guid.ToString();
        }

        // Convert Proto string back to GUID
        public static Guid ProtoStringToGuid(string protoString)
        {
            return Guid.Parse(protoString);
        }
    }
}
