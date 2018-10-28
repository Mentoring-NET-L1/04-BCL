using System.Text.RegularExpressions;

namespace FileManagment
{
    public static class FileHelper
    {
        public static bool IsFileMatchPattern(string filePath, string pattern)
        {
            var mask = new Regex(pattern);
            return mask.IsMatch(filePath);
        }
    }
}
