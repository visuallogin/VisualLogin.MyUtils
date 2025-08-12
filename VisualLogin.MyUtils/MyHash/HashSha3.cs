using System;
using System.IO;
using System.Text;
using VisualLogin.MyUtils.MyBaseExt;

namespace VisualLogin.MyUtils.MyHash
{
    public static class HashSha3
    {

        public static string HashString(string val, HashCase hashCase=HashCase.Lower, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            if (encoding==null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes,0,valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashStringBase64(string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            if (encoding == null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes, 0, valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashStringRaw(string val, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(val)) return Array.Empty<byte>(); 
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            if (encoding == null) encoding = Encoding.UTF8;
            var valBytes = encoding.GetBytes(val);
            cipher.BlockUpdate(valBytes, 0, valBytes.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            return hash;
        }

        public static string HashBytes(byte[] val, HashCase hashCase = HashCase.Lower)
        {
            if (val == null || val.Length == 0) return "";
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            if (hashCase == HashCase.Upper) return hash.ToHexFast(false);
            return hash.ToHexFast();
        }
        public static string HashBytesBase64(byte[] val)
        {
            if (val == null||val.Length==0) return "";
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            var result = Convert.ToBase64String(hash);
            return result;
        }
        public static byte[] HashBytesRaw(byte[] val)
        {
            if (val == null || val.Length == 0) return Array.Empty<byte>();
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
            cipher.BlockUpdate(val, 0, val.Length);
            byte[] hash = new byte[cipher.GetDigestSize()];
            cipher.DoFinal(hash, 0);
            return hash;
        }

        public static string HashStream(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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
        public static string HashStreamBase64(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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
        public static byte[] HashStreamRaw(Stream stream, HashCase hashCase = HashCase.Lower)
        {
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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

        public static string HashFile(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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
        public static string HashFileBase64(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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
        public static byte[] HashFileRaw(string filePath, HashCase hashCase = HashCase.Lower)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var cipher = new Org.BouncyCastle.Crypto.Digests.Sha3Digest();
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
