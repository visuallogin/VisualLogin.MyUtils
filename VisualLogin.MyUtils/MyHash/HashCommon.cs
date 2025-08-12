using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using VisualLogin.MyUtils.MyBaseExt;

namespace VisualLogin.MyUtils.MyHash
{
    public static class HashCommon
    {
        public static string HashString(HashAlgorithm cipher,string val, HashCase hashCase = HashCase.Lower, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            if (encoding == null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();

        }
        public static string HashStringBase64(HashAlgorithm cipher, string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            if (encoding == null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashStringRaw(HashAlgorithm cipher, string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return Array.Empty<byte>(); ;
            if (encoding == null) encoding = Encoding.UTF8;
            var hash = cipher.ComputeHash(encoding.GetBytes(val));
            return hash;
        }

        public static string HashBytes(HashAlgorithm cipher, byte[] val, HashCase hashCase = HashCase.Lower)
        {
            if (val == null || val.Length == 0) return "";
            var hash = cipher.ComputeHash(val);
            if (hashCase == HashCase.Upper) return cipher.Hash.ToHexFast(false);
            return cipher.Hash.ToHexFast();
        }
        public static string HashBytesBase64(HashAlgorithm cipher, byte[] val)
        {
            if (val == null || val.Length == 0) return "";
            var hash = cipher.ComputeHash(val);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashBytesRaw(HashAlgorithm cipher, byte[] val)
        {
            if (val == null || val.Length == 0) return Array.Empty<byte>();
            var hash = cipher.ComputeHash(val);
            return hash;
        }
        public static string HashStream(HashAlgorithm cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
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
        public static string HashStreamBase64(HashAlgorithm cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
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
        public static byte[] HashStreamRaw(HashAlgorithm cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.TransformBlock(buffer, 0, read, null, 0);
            }
            cipher.TransformFinalBlock(buffer, 0, 0); // 完成哈希计算
            return cipher.Hash;
        }

        public static string HashFile(HashAlgorithm cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
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
        public static string HashFileBase64(HashAlgorithm cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
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
        public static byte[] HashFileRaw(HashAlgorithm cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
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


        public static string HashString(IDigest cipher, string val, HashCase hashCase = HashCase.Lower, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            if (encoding == null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes, 0, valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashStringBase64(IDigest cipher, string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            if (encoding == null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes, 0, valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashStringRaw(IDigest cipher, string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return Array.Empty<byte>();
            if (encoding == null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes, 0, valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            return hash;
        }
        public static string HashBytes(IDigest cipher, byte[] val, HashCase hashCase = HashCase.Lower)
        {
            if (val == null || val.Length == 0) return "";
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashBytesBase64(IDigest cipher, byte[] val)
        {
            if (val == null || val.Length == 0) return "";
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashBytesRaw(IDigest cipher, byte[] val)
        {
            if (val == null || val.Length == 0) return Array.Empty<byte>();
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            return hash;
        }
        public static string HashStream(IDigest cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashStreamBase64(IDigest cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashStreamRaw(IDigest cipher, Stream stream, HashCase hashCase = HashCase.Lower)
        {
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            return hash;
        }
        public static string HashFile(IDigest cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            stream.Close();
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashFileBase64(IDigest cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            stream.Close();
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashFileRaw(IDigest cipher, string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            byte[] hash = new byte[cipher.GetDigestSize()];
            byte[] buffer = new byte[8192 * 256]; // 8 KB buffer
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cipher.BlockUpdate(buffer, 0, read);
            }
            cipher.DoFinal(hash, 0); // 完成哈希计算
            stream.Close();
            return hash;
        }

    }
}