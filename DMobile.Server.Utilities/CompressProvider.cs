using System.IO;

namespace DMobile.Server.Utilities
{
    public class CompressProvider
    {
        public static Stream Compress(Stream fromStream)
        {
            return new MemoryStream();
        }

        public static string Compress(string fromString)
        {
            return fromString;
        }

        public static string Decompress(string fromString)
        {
            return fromString;
        }

        public static Stream Decompress(Stream fromStream)
        {
            return new MemoryStream();
        }
    }
}