namespace QiitaDotNet.UnitTests.TestData;

/// <summary>
/// Tag 系テストで共有する JSON データです。
/// </summary>
internal static class TagJson
{
    /// <summary>通常の値を持つタグ 1 件分の JSON です。</summary>
    internal const string Typical = """
        {
            "followers_count": 100,
            "icon_url": "https://example.com/tag.png",
            "id": "qiita",
            "items_count": 200
        }
        """;

    /// <summary>アイコン URL が null のタグ 1 件分の JSON です。</summary>
    internal const string WithNull = """
        {
            "followers_count": 0,
            "icon_url": null,
            "id": "null_icon",
            "items_count": 1
        }
        """;

    /// <summary>
    /// <see cref="Typical"/> と <see cref="WithNull"/> を連結した、JSON 配列リテラルです。
    /// </summary>
    internal const string TagList = $"[{Typical},{WithNull}]";
}
