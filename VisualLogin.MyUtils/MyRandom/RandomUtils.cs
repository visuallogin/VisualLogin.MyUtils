using System;

namespace VisualLogin.MyUtils.MyRandom
{
    public static class RandomUtils
    {
        public static string RndChar(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
        public static int Rnd(int min, int max) => new Random().Next(min, max);
        public static float Rnd(float min, float max) => (float)(min + new Random().NextDouble() * (max - min));
        public static double Rnd(double min, double max) => min + new Random().NextDouble() * (max - min);
    }
}