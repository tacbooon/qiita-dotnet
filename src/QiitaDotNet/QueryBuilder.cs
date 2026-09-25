using System.Text;

namespace QiitaDotNet;

/// <summary>
/// HTTP リクエスト用のクエリパラメータを組み立てるヘルパークラスです。
/// </summary>
internal sealed class QueryBuilder
{
    private readonly StringBuilder _query = new();

    /// <summary>
    /// クエリパラメータを追加します。<c>null</c> の値は追加しません。
    /// </summary>
    /// <param name="name">パラメータ名。</param>
    /// <param name="value">パラメータ値。</param>
    /// <returns>このインスタンス。</returns>
    internal QueryBuilder AddIfNotNull(string name, int? value)
    {
        if (value.HasValue)
        {
            Append(name, value.Value.ToString());
        }
        return this;
    }

    /// <summary>
    /// クエリパラメータを追加します。<c>null</c> の値は追加しません。
    /// </summary>
    /// <param name="name">パラメータ名。</param>
    /// <param name="value">パラメータ値。</param>
    /// <returns>このインスタンス。</returns>
    internal QueryBuilder AddIfNotNull(string name, string? value)
    {
        if (value is not null)
        {
            Append(name, value);
        }
        return this;
    }

    /// <summary>
    /// 組み立てたクエリ文字列を返します。パラメータが 1 つもない場合は空文字列、1 つ異常ある場合は '?' で始まる文字列を返します。
    /// </summary>
    /// <returns>クエリ文字列。</returns>
    public override string ToString() => _query.ToString();

    private void Append(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _query.Append(_query.Length == 0 ? '?' : '&');
        _query.Append(Uri.EscapeDataString(name)).Append('=').Append(Uri.EscapeDataString(value));
    }
}
