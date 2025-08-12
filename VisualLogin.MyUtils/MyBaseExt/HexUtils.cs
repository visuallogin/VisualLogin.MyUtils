using System;
using System.Linq;

namespace VisualLogin.MyUtils.MyBaseExt
{
    public static class HexUtils
    {
        public static string ToHexFast(this byte[] bytes, bool lowercase = true)
        {
            var chars = new char[bytes.Length * 2];
            char offset = lowercase ? 'a' : 'A';

            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                int high = b >> 4;
                int low = b & 0x0F;

                chars[i * 2] = high < 10 ? (char)('0' + high) : (char)(offset + high - 10);
                chars[i * 2 + 1] = low < 10 ? (char)('0' + low) : (char)(offset + low - 10);
            }
            return new string(chars);
        }
        public static string ToHex(this int val, bool lowercase = true)
        {
            return val.ToString(lowercase ? "x8" : "X8");
        }
        public static string ToHex(this uint val, bool lowercase = true)
        {
            return val.ToString(lowercase ? "x8" : "X8");
        }
        public static string ToHex(this ulong val, bool lowercase = true)
        {
            return val.ToString(lowercase ? "x8" : "X8");
        }
        public static string ToHex(this ushort val, bool lowercase = true)
        {
            return val.ToString(lowercase ? "x8" : "X8");
        }
        public static int FromHexToInt(this string val)
        {
            return Convert.ToInt32(val, 16);
        }
        public static uint FromHexToUint(this string val)
        {
            return Convert.ToUInt32(val, 16);
        }
        public static ushort FromHexToUshort(this string val)
        {
            return Convert.ToUInt16(val, 16);
        }
        public static ulong FromHexToUlong(this string val)
        {
            return Convert.ToUInt64(val, 16);
        }
        public static string ToBinary(this byte val)
        {
            return Convert.ToString(val, 2).PadLeft(8, '0');
        }
        public static string ToOctal(this byte value)
        {
            return Convert.ToString(value, 8).PadLeft(3, '0');
        }
        public static byte FromBinaryToByte(this string binary)
        {
            return Convert.ToByte(binary, 2);
        }
        public static byte FromOctalToByte(this string octal)
        {
            return Convert.ToByte(octal, 8);
        }
        public static byte FromHexToByte(this string hex)
        {
            return Convert.ToByte(hex, 16);
        }
        public static byte[] FromHexToBytes(this string hex)
        {
            if (string.IsNullOrEmpty(hex)) return Array.Empty<byte>();
            if (hex.Length >= 2 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X'))
            {
                hex = hex.Substring(2);
            }
            if (hex.Length == 0) return Array.Empty<byte>();
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
}