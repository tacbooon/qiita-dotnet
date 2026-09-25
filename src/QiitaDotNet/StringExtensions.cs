namespace QiitaDotNet;

/// <summary>
/// <see cref="string"/> クラスの拡張メソッド群です。内部実装のためのユーティリティです。
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// 文字列を指定した最大文字数に切り詰めます。
    /// </summary>
    /// <param name="value">対象の文字列。</param>
    /// <param name="maxLength">最大文字数。</param>
    /// <returns>切り詰め後の文字列。</returns>
    /// <remarks>
    /// <para>文字列が最大文字数以下の場合は元の文字列をそのまま返します。文字列が最大文字数を超える場合は末尾を "..." に置き換えて、全体の文字数を最大文字数以下に収めます。</para>
    /// <para>この関数は、文字数の判定に <see cref="string.Length"/> を使用します。そのため、全角・半角のような表示幅は考慮しません。また、サロゲートペアを正しく認識して分断を防止する機能はありません。</para>
    /// </remarks>
    internal static string Truncate(this string value, int maxLength)
    {
        const string ellipsis = "...";
        if (value.Length <= maxLength)
        {
            return value;
        }
        return maxLength <= ellipsis.Length
            ? ellipsis[..maxLength]
            : value[..(maxLength - ellipsis.Length)] + ellipsis;
    }
}
