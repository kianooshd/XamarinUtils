using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tools
{
    public static class ObjectUtils
    {
        public static string ObjectToString(object obj)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static object StringToObject(string base64String)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    return new BinaryFormatter().Deserialize(ms);
                }
            }
            catch//(Exception ex)
            {
                return null;
            }
        }

        public static byte[] StringToBytes(string text)
        {
            if (text == string.Empty) return null;

            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, text);
                return ms.ToArray();
            }
        }

        public static string BytesToString(byte[] bytes)
        {
            if (bytes == null) return string.Empty;

            using (var ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return (string)new BinaryFormatter().Deserialize(ms);
            }
        }

        public static byte[] ObjectToBytes(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object BytesToObject(byte[] bytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);

            return obj;
        }
    }
}
