using System.IO;
using System.Text;

namespace VisualLogin.MyUtils.MyFileExt
{
    public static class FileUtils
    {
        public static string ReadAllText(this string filepath)
        {
            return File.ReadAllText(filepath);
        }
        public static string ReadAllText(this string filepath, Encoding encoding)
        {
            return File.ReadAllText(filepath, encoding);
        }
        public static string[] ReadAllLines(this string filepath)
        {
            return File.ReadAllLines(filepath);
        }
        public static string[] ReadAllLines(this string filepath, Encoding encoding)
        {
            return File.ReadAllLines(filepath, encoding);
        }
    }
}