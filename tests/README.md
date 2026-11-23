# テストの実行

`App.Tests` は単体テストとSQLiteを使う結合テスト、`App.E2ETests` は実際のChromiumでMVC画面を操作するE2Eテストです。どちらも `net8.0` を対象にします。

## 単体・結合テスト

```powershell
dotnet test App.Tests/App.Tests.csproj
dotnet test App.Tests/App.Tests.csproj --filter "Category=Unit"
dotnet test App.Tests/App.Tests.csproj --filter "Category=Integration"
```

結合テストとE2EテストはSQL Server向けMigrationを実行せず、開いたままのSQLiteインメモリ接続を使います。

## E2Eテスト

```powershell
dotnet build App.E2ETests/App.E2ETests.csproj
powershell -ExecutionPolicy Bypass -File App.E2ETests/bin/Debug/net8.0/playwright.ps1 install chromium
dotnet test App.E2ETests/App.E2ETests.csproj --filter "Category=E2E"
```

PlaywrightのChromiumをすでに準備済みの場合はインストール手順を省略できます。E2E用アプリは `Testing` 環境でループバックの空きポートを使い、ケースごとにブラウザーコンテキストを作成します。共有SQLiteデータベースは直列実行され、ケースの前後にデータを削除します。
