using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VisualLogin.MyUtils.MyBaseExt;

namespace VisualLogin.MyUtils.MyHash
{
    public static class HashSha256
    {
     
        public static string HashString(string val, HashCase hashCase=HashCase.Lower, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            var cipher = SHA256.Create();
            if (encoding==null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();
        }
        public static string HashStringBase64(string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            var cipher = SHA256.Create();
            if (encoding == null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashStringRaw(string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return Array.Empty<byte>(); ;
            var cipher = SHA256.Create();
            if (encoding == null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            return hash;
        }

        public static string HashBytes(byte[] val, HashCase hashCase = HashCase.Lower)
        {
            if (val == null || val.Length == 0) return "";
            var cipher = SHA256.Create();
            var hash = cipher.ComputeHash(val);
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();
        }
        public static string HashBytesBase64(byte[] val)
        {
            if (val == null||val.Length==0) return "";
            var cipher = SHA256.Create();
            var hash = cipher.ComputeHash(val);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashBytesRaw(byte[] val)
        {
            if (val == null || val.Length == 0) return Array.Empty<byte>();
            var cipher = SHA256.Create();
            var hash = cipher.ComputeHash(val);
            return hash;
        }

        public static string HashStream(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();
        }
        public static string HashStreamBase64(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            var result = Convert.ToBase64String(cipher.Hash);
            return result;
        }
        public static byte[] HashStreamRaw(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            return cipher.Hash;
        }

         public static string HashFile(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            stream.Close();
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();
        }
        public static string HashFileBase64(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            stream.Close();
            var result = Convert.ToBase64String(cipher.Hash);
            return result;
        }
        public static byte[] HashFileRaw(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = SHA256.Create();
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            stream.Close();
            return cipher.Hash;
        }


    }
}
