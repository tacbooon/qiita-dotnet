namespace QiitaDotNet.UnitTests.TestData;

/// <summary>
/// エラー系テストで共有する JSON データです。
/// </summary>
internal static class ErrorJson
{
    /// <summary>存在しないリソースへのアクセス時のエラー応答の JSON です。</summary>
    internal const string NotFound = """
        {
            "message": "Not found",
            "type": "not_found"
        }
        """;

    /// <summary><c>type</c> が欠けた不完全なエラー応答の JSON です。</summary>
    internal const string MissingType = """
        {
            "message": "Missing Type"
        }
        """;

    /// <summary><c>message</c> が欠けた不完全なエラー応答の JSON です。</summary>
    internal const string MissingMessage = """
        {
            "type": "not_found"
        }
        """;
}
