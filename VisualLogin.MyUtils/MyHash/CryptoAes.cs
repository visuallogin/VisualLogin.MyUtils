using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace VisualLogin.MyUtils.MyHash
{
    public static class CryptoAes
    {
        /// <summary>
        ///     流式加密
        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <param name="key">密钥 (16, 24, 或 32 字节)</param>
        /// <param name="iv">初始化向量 (16 字节)</param>
        /// <param name="mode">加密模式</param>
        /// <param name="padding">填充模式</param>
        public static void EncryptStream(Stream inputStream, Stream outputStream, byte[] key, byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null) throw new ArgumentNullException(nameof(outputStream));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!outputStream.CanWrite) throw new ArgumentException("输出流必须可写", nameof(outputStream));
            if (!inputStream.CanRead) throw new ArgumentException("输入流必须可读", nameof(inputStream));

            // 验证密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            aes.Padding = padding;
            var cs = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            var buffer = new byte[8192]; // 8 KB buffer
            int read;
            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0) cs.Write(buffer, 0, read);
            cs.FlushFinalBlock();
            cs.Close();
            cs.Dispose();
            outputStream.Flush();
        }

        /// <summary>
        ///     加密字节数组
        /// </summary>
        /// <param name="inputBytes">待加密的输入字节数组</param>
        /// <param name="outputBytes">加密后的输出字节数组（通过out参数返回）</param>
        /// <param name="key">加密密钥字节数组</param>
        /// <param name="iv">初始化向量字节数组，对于CBC等模式是必需的（可选参数，默认为null）</param>
        /// <param name="mode">加密模式，如CBC、ECB等（可选参数，默认为CBC模式）</param>
        /// <param name="padding">填充模式，如PKCS7、None等（可选参数，默认为PKCS7填充）</param>
        public static void EncryptBytes(byte[] inputBytes, out byte[] outputBytes, byte[] key, byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (inputBytes == null) throw new ArgumentNullException(nameof(inputBytes));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            aes.Padding = padding;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.FlushFinalBlock();
            cs.Close();
            cs.Dispose();
            outputBytes = ms.ToArray();
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        ///     流式解密
        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <param name="key">密钥 (16, 24, 或 32 字节)</param>
        /// <param name="iv">初始化向量 (16 字节)</param>
        /// <param name="mode">加密模式</param>
        /// <param name="padding">填充模式</param>
        public static void DecryptStream(Stream inputStream, Stream outputStream, byte[] key, byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null) throw new ArgumentNullException(nameof(outputStream));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!outputStream.CanWrite) throw new ArgumentException("输出流必须可写", nameof(outputStream));
            if (!inputStream.CanRead) throw new ArgumentException("输入流必须可读", nameof(inputStream));

            // 验证密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            aes.Padding = padding;
            var cs = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
            var buffer = new byte[8192]; // 8 KB buffer
            int read;
            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0) cs.Write(buffer, 0, read);
            cs.FlushFinalBlock();
            cs.Close();
            cs.Dispose();
            outputStream.Flush();
        }

        /// <summary>
        ///     解密字节数组
        /// </summary>
        /// <param name="inputBytes">待解密的输入字节数组</param>
        /// <param name="outputBytes">解密后的输出字节数组（通过out参数返回）</param>
        /// <param name="key">解密密钥字节数组</param>
        /// <param name="iv">初始化向量字节数组，对于CBC等模式是必需的（可选参数，默认为null）</param>
        /// <param name="mode">加密模式，如CBC、ECB等（可选参数，默认为CBC模式）</param>
        /// <param name="padding">填充模式，如PKCS7、None等（可选参数，默认为PKCS7填充）</param>
        public static void DecryptBytes(byte[] inputBytes, out byte[] outputBytes, byte[] key, byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (inputBytes == null) throw new ArgumentNullException(nameof(inputBytes));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = mode;
            aes.Padding = padding;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.FlushFinalBlock();
            cs.Close();
            cs.Dispose();
            outputBytes = ms.ToArray();
            ms.Close();
            ms.Dispose();
        }

        public static void EncryptStreamByBouncyCastle(Stream inputStream, Stream outputStream, byte[] key,
            byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, int cfbFeedbackBits = 128,
            int ofbFeedbackBits = 128)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null) throw new ArgumentNullException(nameof(outputStream));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!outputStream.CanWrite) throw new ArgumentException("输出流必须可写", nameof(outputStream));
            if (!inputStream.CanRead) throw new ArgumentException("输入流必须可读", nameof(inputStream));

            // 验证密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            // 创建 BouncyCastle AES 引擎
            var aesEngine = new AesEngine();

            // 根据模式选择包装器
            IBlockCipher blockCipher = null;
            switch (mode)
            {
                case CipherMode.CBC:
                    blockCipher = new CbcBlockCipher(aesEngine);
                    break;
                case CipherMode.ECB:
                    blockCipher = new EcbBlockCipher(aesEngine);
                    break;
                case CipherMode.CFB:
                    blockCipher = new CfbBlockCipher(aesEngine, cfbFeedbackBits);
                    break;
                case CipherMode.CTS:
                    throw new NotSupportedException("CTS 模式需要特殊处理，暂不支持");
                case CipherMode.OFB:
                    blockCipher = new OfbBlockCipher(aesEngine, ofbFeedbackBits);
                    break;
                default:
                    throw new NotSupportedException($"不支持的加密模式: {mode}");
            }

            IBufferedCipher cipher = null;
            switch (padding)
            {
                case PaddingMode.None:
                    cipher = new BufferedBlockCipher(blockCipher);
                    break;
                case PaddingMode.PKCS7:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                    break;
                case PaddingMode.Zeros:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
                    break;
                case PaddingMode.ANSIX923:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new X923Padding());
                    break;
                case PaddingMode.ISO10126:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new ISO10126d2Padding());
                    break;
                default:
                    throw new NotSupportedException($"不支持的填充模式: {padding}");
            }


            // 设置密钥和 IV
            var keyParam = new KeyParameter(key);
            var keyParamWithIV = new ParametersWithIV(keyParam, iv);

            // 初始化加密器
            cipher.Init(true, keyParamWithIV); // true 表示加密

            var buffer = new byte[8192]; // 8 KB buffer
            var outputBuffer = new byte[cipher.GetOutputSize(8192)];

            int read;
            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var outputLen = cipher.ProcessBytes(buffer, 0, read, outputBuffer, 0);
                if (outputLen > 0) outputStream.Write(outputBuffer, 0, outputLen);
            }

            // 处理最后的块
            var finalOutputLen = cipher.DoFinal(outputBuffer, 0);
            if (finalOutputLen > 0) outputStream.Write(outputBuffer, 0, finalOutputLen);

            outputStream.Flush();
        }

        public static void DecryptStreamByBouncyCastle(Stream inputStream, Stream outputStream, byte[] key,
            byte[] iv = null,
            CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, int cfbFeedbackBits = 128,
            int ofbFeedbackBits = 128)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null) throw new ArgumentNullException(nameof(outputStream));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!outputStream.CanWrite) throw new ArgumentException("输出流必须可写", nameof(outputStream));
            if (!inputStream.CanRead) throw new ArgumentException("输入流必须可读", nameof(inputStream));

            // 验证密钥长度
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));
            if (iv == null) iv = new byte[16];
            if (iv.Length != 16)
                throw new ArgumentException("IV 长度必须为 16 字节", nameof(iv));

            // 创建 BouncyCastle AES 引擎
            var aesEngine = new AesEngine();

            // 根据模式选择包装器
            IBlockCipher blockCipher = null;
            switch (mode)
            {
                case CipherMode.CBC:
                    blockCipher = new CbcBlockCipher(aesEngine);
                    break;
                case CipherMode.ECB:
                    blockCipher = new EcbBlockCipher(aesEngine);
                    break;
                case CipherMode.CFB:
                    blockCipher = new CfbBlockCipher(aesEngine, cfbFeedbackBits);
                    break;
                case CipherMode.CTS:
                    throw new NotSupportedException("CTS 模式需要特殊处理，暂不支持");
                case CipherMode.OFB:
                    blockCipher = new OfbBlockCipher(aesEngine, ofbFeedbackBits);
                    break;
                default:
                    throw new NotSupportedException($"不支持的加密模式: {mode}");
            }

            IBufferedCipher cipher = null;
            switch (padding)
            {
                case PaddingMode.None:
                    cipher = new BufferedBlockCipher(blockCipher);
                    break;
                case PaddingMode.PKCS7:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                    break;
                case PaddingMode.Zeros:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
                    break;
                case PaddingMode.ANSIX923:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new X923Padding());
                    break;
                case PaddingMode.ISO10126:
                    cipher = new PaddedBufferedBlockCipher(blockCipher, new ISO10126d2Padding());
                    break;
                default:
                    throw new NotSupportedException($"不支持的填充模式: {padding}");
            }


            // 设置密钥和 IV
            var keyParam = new KeyParameter(key);
            var keyParamWithIV = new ParametersWithIV(keyParam, iv);

            // 初始化加密器
            cipher.Init(false, keyParamWithIV); // true 表示加密

            var buffer = new byte[8192]; // 8 KB buffer
            var outputBuffer = new byte[cipher.GetOutputSize(8192)];

            int read;
            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var outputLen = cipher.ProcessBytes(buffer, 0, read, outputBuffer, 0);
                if (outputLen > 0) outputStream.Write(outputBuffer, 0, outputLen);
            }

            // 处理最后的块
            var finalOutputLen = cipher.DoFinal(outputBuffer, 0);
            if (finalOutputLen > 0) outputStream.Write(outputBuffer, 0, finalOutputLen);

            outputStream.Flush();
        }
    }
}