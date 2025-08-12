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
    }
}