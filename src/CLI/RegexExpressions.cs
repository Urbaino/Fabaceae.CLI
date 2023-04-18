namespace Fabaceae.CLI;

using System.Text.RegularExpressions;

/// <summary>
/// Holds compiled regex expressions.
/// </summary>
internal static partial class RegexExpressions
{
    [GeneratedRegex("^account (?:(\\w+):?)+(?!:)$")]
    public static partial Regex Account();
}