using System.Text.RegularExpressions;

namespace HuiTrade.Infrastructure;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // 使用正则：在小写字母和大写字母之间插入下划线，然后转小写
        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}