namespace QiitaDotNet;

/// <summary>
/// ページネーション用パラメータの検証です。Qiita API の一覧取得系エンドポイントでは共通で <c>page</c>, <c>per_page</c> を使います。
/// </summary>
internal static class Pagination
{
    /// <summary>
    /// ページ番号 (<c>page</c>) の最小値です。
    /// </summary>
    internal const int MinPage = 1;

    /// <summary>
    /// ページ番号 (<c>page</c>) の最大値です。
    /// </summary>
    internal const int MaxPage = 100;

    /// <summary>
    /// 1ページあたりの件数 (<c>per_page</c>) の最小値です。
    /// </summary>
    internal const int MinPerPage = 1;

    /// <summary>
    /// 1ページあたりの件数 (<c>per_page</c>) の最大値です。
    /// </summary>
    internal const int MaxPerPage = 100;

    /// <summary>
    /// <c>page</c>, <c>per_page</c> が有効範囲内であることを検証します。
    /// </summary>
    /// <param name="page">ページ番号。<c>null</c> は未指定を意味する有効値。</param>
    /// <param name="perPage">1ページあたりの件数。<c>null</c> は未指定を意味する有効値。</param>
    /// <exception cref="ArgumentOutOfRangeException">引数の値が範囲外。</exception>
    internal static void Validate(int? page, int? perPage)
    {
        if (page is < MinPage or > MaxPage)
        {
            throw new ArgumentOutOfRangeException(
                nameof(page), page, $"page must be between {MinPage} and {MaxPage}.");
        }
        if (perPage is < MinPerPage or > MaxPerPage)
        {
            throw new ArgumentOutOfRangeException(
                nameof(perPage), perPage, $"perPage must be between {MinPerPage} and {MaxPerPage}.");
        }
    }
}
