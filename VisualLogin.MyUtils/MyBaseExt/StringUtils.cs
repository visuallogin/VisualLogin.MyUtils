using System;
using System.IO;
using System.Linq;

namespace VisualLogin.MyUtils.MyBaseExt
{
    public static class StringUtils
    {
        public static string Escape(this string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            return new string(id.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
        public static string EscapePath_(this string id)
        {
            return string.IsNullOrEmpty(id) ? string.Empty : new string(id.Select(c => Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).Contains(c) || char.IsWhiteSpace(c) ? '_' : c).ToArray());
        }
        public static string EscapePath(this string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            var invalidChars = Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).Distinct().ToList();
            return new string(id.Where(c => !invalidChars.Contains(c) && !char.IsWhiteSpace(c)).ToArray());
        }

        public static string[] SplitLines(this string val,bool removeEmpty=true)
        {
            if (string.IsNullOrEmpty(val)) return null;
            if (removeEmpty)
            {
                return val.Replace("\r", "").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            return val.Replace("\r", "").Split('\n');

        }
    }
}