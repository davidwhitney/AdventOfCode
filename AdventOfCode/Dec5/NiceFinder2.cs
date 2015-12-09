using System.Text.RegularExpressions;

namespace AdventOfCode.Dec5
{
    public class NiceFinder2
    {
        public bool IsNice(string s)
        {
            return Regex.IsMatch(s, @"(..).*\1") && Regex.IsMatch(s, @"(.).\1");
        }
    }
}