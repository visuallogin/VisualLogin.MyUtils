using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using PemReader = Org.BouncyCastle.OpenSsl.PemReader;
using PemWriter = Org.BouncyCastle.OpenSsl.PemWriter;

namespace VisualLogin.MyUtils.MyHash
{
    public static class CryptoRsa
    {
        /// <summary>
        /// 生成 RSA 密钥对
        /// </summary>
        /// <param name="keySize">密钥长度（通常为 1024、2048、4096）</param>
        /// <returns>包含公钥和私钥的元组</returns>
        public static (string publicKey, string privateKey) GenerateKeyPairPkcs1(int keySize = 2048)
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            var keyPair = rsaKeyPairGenerator.GenerateKeyPair();

            // 转换为 PEM 格式
            var publicKey = ExportPublicKeyToPemPkcs1((RsaKeyParameters)keyPair.Public);
            var privateKey = ExportPrivateKeyToPemPkcs1((RsaPrivateCrtKeyParameters)keyPair.Private);

            return (publicKey, privateKey);
        }
        /// <summary>
        /// 生成 RSA 密钥对
        /// </summary>
        /// <param name="keySize">密钥长度（通常为 1024、2048、4096）</param>
        /// <returns>包含公钥和私钥的元组</returns>
        public static (string publicKey, string privateKey) GenerateKeyPairPkcs8(int keySize = 2048)
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            var keyPair = rsaKeyPairGenerator.GenerateKeyPair();

            // 转换为 PEM 格式
            var publicKey = ExportPublicKeyToPemPkcs8((RsaKeyParameters)keyPair.Public);
            var privateKey = ExportPrivateKeyToPemPkcs8((RsaPrivateCrtKeyParameters)keyPair.Private);

            return (publicKey, privateKey);
        }


        public static (string publicKeyPkcs1, string privateKeyPkcs1, string publicKeyPkcs8, string privateKeyPkcs8)
            GenerateKeyPair(int keySize = 2048)
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            var keyPair = rsaKeyPairGenerator.GenerateKeyPair();
            var publicKeyPkcs8 = ExportPublicKeyToPemPkcs8((RsaKeyParameters)keyPair.Public);
            var privateKeyPkcs8 = ExportPrivateKeyToPemPkcs8((RsaPrivateCrtKeyParameters)keyPair.Private);

            var publicKeyPkcs1 = ExportPublicKeyToPemPkcs1((RsaKeyParameters)keyPair.Public);
            var privateKeyPkcs1 = ExportPrivateKeyToPemPkcs1((RsaPrivateCrtKeyParameters)keyPair.Private);
            return ( publicKeyPkcs1,  privateKeyPkcs1,  publicKeyPkcs8,  privateKeyPkcs8);
        }

        /// <summary>
        /// 将私钥导出为 PEM 格式 PKCS#8
        /// </summary>
        public static string ExportPrivateKeyToPemPkcs8(RsaPrivateCrtKeyParameters privateKey)
        {
            // var privateKeyInfo = Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            // var privateKeyBytes = privateKeyInfo.GetDerEncoded();
            // var base64String = Convert.ToBase64String(privateKeyBytes, 0, privateKeyBytes.Length, Base64FormattingOptions.InsertLineBreaks);
            // var pkcs8PrivateKey= $"-----BEGIN PRIVATE KEY-----\n{base64String}\n-----END PRIVATE KEY-----";
            // return pkcs8PrivateKey;
            using (var writer = new StringWriter())
            {
                using (var pemWriter = new PemWriter(writer))
                {
                    var pkcs8Generator = new Pkcs8Generator(privateKey);
                    pemWriter.WriteObject(pkcs8Generator);
                }
                return writer.ToString();
            }

        }
        /// <summary>
        /// 将公钥导出为 PEM 格式 PKCS#8
        /// </summary>
        public static string ExportPublicKeyToPemPkcs8(AsymmetricKeyParameter publicKey)
        {
            // var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            // var publicKeyBytes = publicKeyInfo.GetDerEncoded();
            // var base64String = Convert.ToBase64String(publicKeyBytes, 0, publicKeyBytes.Length, Base64FormattingOptions.InsertLineBreaks);
            // var pkcs8PublicKey = $"-----BEGIN PUBLIC KEY-----\n{base64String}\n-----END PUBLIC KEY-----";
            // return pkcs8PublicKey;
            using (var writer = new StringWriter())
            {
                using (var pemWriter = new PemWriter(writer))
                {
                    var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                    pemWriter.WriteObject(publicKeyInfo);
                }
                return writer.ToString();
            }
        }
        /// <summary>
        /// 将私钥导出为 PEM 格式 PKCS#1
        /// </summary>
        public static string ExportPrivateKeyToPemPkcs1(RsaPrivateCrtKeyParameters privateKey)
        {
            using (var writer = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                var rsaPrivateKey = new RsaPrivateKeyStructure(
                    privateKey.Modulus,
                    privateKey.PublicExponent,
                    privateKey.Exponent,
                    privateKey.P,
                    privateKey.Q,
                    privateKey.DP,
                    privateKey.DQ,
                    privateKey.QInv
                );
                var pemObject = new PemObject("RSA PRIVATE KEY", rsaPrivateKey.GetEncoded());
                pemWriter.WriteObject(pemObject);
                return writer.ToString();
                // var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                // pemWriter.WriteObject(privateKey);
                // return writer.ToString();
            }
        }
        /// <summary>
        /// 将公钥导出为 PEM 格式 PKCS#1
        /// </summary>
        public static string ExportPublicKeyToPemPkcs1(RsaKeyParameters publicKey)
        {

            using (var writer = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                var rsaPublicKey = new RsaPublicKeyStructure(publicKey.Modulus, publicKey.Exponent);
                var pemObject = new PemObject("RSA PUBLIC KEY", rsaPublicKey.GetEncoded());
                pemWriter.WriteObject(pemObject);
                return writer.ToString();
            }

            // using (var writer = new StringWriter())
            // {
            //     var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
            //     pemWriter.WriteObject(publicKey);
            //     return writer.ToString();
            // }
        }


        /// <summary>
        /// 从 PEM 格式导入公钥
        /// </summary>
        public static AsymmetricKeyParameter ImportPublicKeyFromPemPkcs8(string publicKeyPem)
        {
            var publicKeyBytes = Convert.FromBase64String(
                publicKeyPem.Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
            );
            var publicKeyInfo = SubjectPublicKeyInfo.GetInstance(publicKeyBytes);
            AsymmetricKeyParameter key= PublicKeyFactory.CreateKey(publicKeyInfo);
            return key;
        }

        /// <summary>
        /// 从 PEM 格式导入公钥
        /// </summary>
        public static object ImportPrivateKeyFromPemPkcs8(string privateKeyPem)
        {
            var privateKeyBytes = Convert.FromBase64String(
                privateKeyPem.Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
            );
            using (var ms = new MemoryStream(privateKeyBytes))
            {
                var pemReader = new PemReader(new StreamReader(ms));
                var privateKey = pemReader.ReadObject();
                return privateKey;
            }
        }

        public static object ReadKey(string privateKeyPem)
        {
            try
            {
                using (var stringReader = new StringReader(privateKeyPem))
                {
                    var pemReader = new PemReader(stringReader);
                    var keyObject = pemReader.ReadObject();

                    if (keyObject == null)
                    {
                        throw new InvalidOperationException("无法解析 - 可能是格式不正确");
                    }

                    return keyObject;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    return OpenSshPublicKeyUtilities.ParsePublicKey(CleanPem(privateKeyPem));
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"解析时出错: {ex.Message}", e);
                }
               
              
            }
        }
        /// <summary>
        /// 从 PEM 格式导入公钥
        /// </summary>
        public static RsaPrivateCrtKeyParameters ImportPrivateKeyFromPemPkcs1(string privateKeyPem)
        {
            var privateKeyBytes = Convert.FromBase64String(
                privateKeyPem.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
            );
            // 解析 PKCS#1 格式的私钥
            RsaPrivateKeyStructure rsaPrivateKey = RsaPrivateKeyStructure.GetInstance(
                Org.BouncyCastle.Asn1.Asn1Object.FromByteArray(privateKeyBytes));
            var keyParameters = new RsaPrivateCrtKeyParameters(
                rsaPrivateKey.Modulus,
                rsaPrivateKey.PublicExponent,
                rsaPrivateKey.PrivateExponent,
                rsaPrivateKey.Prime1,
                rsaPrivateKey.Prime2,
                rsaPrivateKey.Exponent1,
                rsaPrivateKey.Exponent2,
                rsaPrivateKey.Coefficient);
            return keyParameters;
        }
        /// <summary>
        /// 从 PEM 格式导入公钥
        /// </summary>
        public static AsymmetricKeyParameter ImportPublicKeyFromPemPkcs1(string publicKeyPem)
        {
            var publicKeyBytes = Convert.FromBase64String(
                publicKeyPem.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
            );
            var keyParameter = PublicKeyFactory.CreateKey(publicKeyBytes);
            return keyParameter;
        }

        public static string PemPublicKeyToXml(string pem)
        {
            var rsa = RSA.Create();
            var key = ReadKey(pem);
            if (key is  Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters pub )
            {
                var rsaParameters = new RSAParameters
                {
                    Modulus = pub.Modulus.ToByteArrayUnsigned(),
                    Exponent = pub.Exponent.ToByteArrayUnsigned()
                };

                rsa.ImportParameters(rsaParameters);
                return rsa.ToXmlString(false);
            }
            return "";
        }
        public static string PemPrivateKeyToXml(string pem)
        {
            var rsa = RSA.Create();
            var key = ReadKey(pem);
            if (key is Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair pri)
            {
                if (pri.Private is Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters p)
                {
                    var rsaParameters = new RSAParameters
                    {
                        Modulus = p.Modulus.ToByteArrayUnsigned(),
                        Exponent = p.PublicExponent.ToByteArrayUnsigned(), // 公钥指数 (e)
                        D = p.Exponent.ToByteArrayUnsigned(),              // 私钥指数 (d)
                        P = p.P.ToByteArrayUnsigned(),
                        Q = p.Q.ToByteArrayUnsigned(),
                        DP = p.DP.ToByteArrayUnsigned(),
                        DQ = p.DQ.ToByteArrayUnsigned(),
                        InverseQ = p.QInv.ToByteArrayUnsigned(),
                    };
                    rsa.ImportParameters(rsaParameters);
                    return rsa.ToXmlString(true);
                }
            }
            else if (key is Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters p)
            {
                var rsaParameters = new RSAParameters
                {
                    Modulus = p.Modulus.ToByteArrayUnsigned(),
                    Exponent = p.PublicExponent.ToByteArrayUnsigned(), // 公钥指数 (e)
                    D = p.Exponent.ToByteArrayUnsigned(),              // 私钥指数 (d)
                    P = p.P.ToByteArrayUnsigned(),
                    Q = p.Q.ToByteArrayUnsigned(),
                    DP = p.DP.ToByteArrayUnsigned(),
                    DQ = p.DQ.ToByteArrayUnsigned(),
                    InverseQ = p.QInv.ToByteArrayUnsigned(),
                };
                rsa.ImportParameters(rsaParameters);
                return rsa.ToXmlString(true);
            }
            return "";
        }

        public static string XmlPublicKeyToPemPkcs1(string xml)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(xml);
            var rsaParameters = rsa.ExportParameters(false);
            var modulus = new BigInteger(1, rsaParameters.Modulus);
            var exponent = new BigInteger(1, rsaParameters.Exponent);
            using (var writer = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                var rsaPublicKey = new RsaPublicKeyStructure(modulus, exponent);
                var pemObject = new PemObject("RSA PUBLIC KEY", rsaPublicKey.GetEncoded());
                pemWriter.WriteObject(pemObject);
                return writer.ToString();
            }
        }
        public static string XmlPublicKeyToPemPkcs8(string xml)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(xml);
            var rsaParameters = rsa.ExportParameters(false);
            var modulus = new Org.BouncyCastle.Math.BigInteger(1, rsaParameters.Modulus);
            var exponent = new Org.BouncyCastle.Math.BigInteger(1, rsaParameters.Exponent);

            using (var writer = new StringWriter())
            {
                using (var pemWriter = new PemWriter(writer))
                {
                    var rsaPublicKey = new RsaKeyParameters(false, modulus, exponent);
                    var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(rsaPublicKey);
                    pemWriter.WriteObject(publicKeyInfo);
                }
                return writer.ToString();
            }
        }

        public static string XmlPrivateKeyToPemPkcs1(string xml)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(xml);
            RSAParameters privateKey = rsa.ExportParameters(true);
            using (var writer = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                var rsaPrivateKey = new RsaPrivateKeyStructure(
                    new BigInteger(1, privateKey.Modulus),
                    new BigInteger(1, privateKey.Exponent),
                    new BigInteger(1, privateKey.D),
                    new BigInteger(1, privateKey.P),
                    new BigInteger(1, privateKey.Q),
                    new BigInteger(1, privateKey.DP),
                    new BigInteger(1, privateKey.DQ),
                    new BigInteger(1, privateKey.InverseQ)
                );
                var pemObject = new PemObject("RSA PRIVATE KEY", rsaPrivateKey.GetEncoded());
                pemWriter.WriteObject(pemObject);
                return writer.ToString();
            }
        }
        public static string XmlPrivateKeyToPemPkcs8(string xml)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(xml);
            RSAParameters privateKey = rsa.ExportParameters(true);
            using (var writer = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(writer);
                var rsaPrivateCrtKey = new RsaPrivateCrtKeyParameters(
                    new BigInteger(1, privateKey.Modulus),
                    new BigInteger(1, privateKey.Exponent),
                    new BigInteger(1, privateKey.D),
                    new BigInteger(1, privateKey.P),
                    new BigInteger(1, privateKey.Q),
                    new BigInteger(1, privateKey.DP),
                    new BigInteger(1, privateKey.DQ),
                    new BigInteger(1, privateKey.InverseQ)
                );
                var pkcs8Generator = new Pkcs8Generator(rsaPrivateCrtKey);
                pemWriter.WriteObject(pkcs8Generator);
                return writer.ToString();
            }
        }

        public static byte[] CleanPem(string pem)
        {
            StringBuilder sb= new StringBuilder();
            var lines= pem.Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("-"))continue;
                if (line.StartsWith("Comment:")) continue;
                if (string.IsNullOrWhiteSpace(line))continue;
                sb.Append(line.Trim());
            }
            return Convert.FromBase64String(sb.ToString());
        }
    }
}