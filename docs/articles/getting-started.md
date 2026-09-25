# クイックスタート

## 最小コード

認証なしでユーザー一覧を取得する例です。

```csharp
using QiitaDotNet;

var httpClient = new HttpClient();
var client = new QiitaClient(httpClient);

var users = await client.Users.ListUsersAsync();
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.Name}");
}
```

@System.Net.Http.HttpClient に `BaseAddress` が未設定の場合は `https://qiita.com/` に対してリクエストを送信します。Qiita Team を使う場合は `BaseAddress` に `https://<team-id>.qiita.com/` を設定してください。`api/v2` は含めないでください。

認証なしでは利用可能な機能や呼び出し回数が制限されます。本番環境では必ず[認証](authentication.md)を行ってください。

> [!NOTE]
> @System.Net.Http.HttpClient の寿命管理は呼び出し元の責任です。@QiitaDotNet.QiitaClient は渡された @System.Net.Http.HttpClient を破棄しません。

## ページング

Qiita API v2 では一覧を取得する多くの API がページネーションをサポートしています。

例えば @QiitaDotNet.Users.IUsersClient が提供する `ListUsersAsync` メソッドがこれに該当します。これらのメソッドは `page`（1〜100）と `perPage`（1〜100）のオプション引数を持ちます。省略時はサーバー既定値 (`page` は 1、`perPage` は 20) が使われます。範囲外の値を渡すと @System.ArgumentException がスローされます。

以下の例では、作成日時が新しいユーザから 11 〜 20 人目までの 10 件を取得します。

```csharp
using QiitaDotNet;

var httpClient = new HttpClient();
var client = new QiitaClient(httpClient);
var users = await client.Users.ListUsersAsync(page: 2, perPage: 10);
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.Name}");
}
```

## キャンセル

キャンセルやタイムアウトには @System.Threading.CancellationToken を使用してください。

```csharp
using QiitaDotNet;

var httpClient = new HttpClient();
var client = new QiitaClient(httpClient);

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var users = await client.Users.ListUsersAsync(cancellationToken: cts.Token);
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.Name}");
}
```

## エラーハンドリング

API 呼び出しに失敗したときは以下のいずれかの例外がスローされます。

| 例外                                          | 発生要因                                      |
|-----------------------------------------------|-----------------------------------------------|
| @System.ArgumentException                     | 引数が無効                                    |
| @System.Net.Http.HttpRequestException         | ネットワークエラーが発生                      |
| @System.InvalidOperationException             | URL に起因するトークン送信ブロック            |
| @System.Text.Json.JsonException               | レスポンスボディが不正な JSON                 |
| @System.OperationCanceledException            | キャンセルまたはタイムアウト                  |
| @QiitaDotNet.QiitaApiException                | Qiita サーバーからエラーレスポンスを受信      |

特に @QiitaDotNet.QiitaApiException からは Qiita API v2 固有の情報として `ErrorType` を取得できます。

```csharp
using QiitaDotNet;
using System.Text.Json;

var httpClient = new HttpClient();
var client = new QiitaClient(httpClient);

try
{
    var users = await client.Users.ListUsersAsync();
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.Name}");
    }
}
catch (QiitaApiException ex)
{
    Console.WriteLine($"API failed: {ex.ErrorType}");
}
catch (HttpRequestException)
{
    Console.WriteLine("Network error");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Timed out or cancelled");
}
catch (JsonException)
{
    Console.WriteLine("Invalid response");
}
```

> [!NOTE]
> @QiitaDotNet.QiitaApiException は @System.Net.Http.HttpRequestException の派生クラスです。@System.Net.Http.HttpRequestException より先に catch してください。
