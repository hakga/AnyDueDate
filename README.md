# NET8WebMVC - 接続文字列の扱い（ユーザーシークレットへ移動）

このリポジトリでは、機密情報や環境依存の接続文字列をソース管理に含めないようにしています。開発環境で接続文字列を設定する手順は以下のとおりです。

1. Visual Studio で設定する方法（推奨）
   - ソリューションエクスプローラーでプロジェクト `NET8WebMVC` を右クリック
   - [Manage User Secrets] を選択すると、usersecrets.json が開きます
   - 次のように接続文字列を追加します：
	 {
	   "ConnectionStrings": {
		 "DefaultConnection": "<あなたの接続文字列>"
	   }
	 }

2. dotnet CLI を使う方法
   - リポジトリルートから次のコマンドを実行します（PowerShell）:
	 cd NET8WebMVC
	 dotnet user-secrets init
	 dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<あなたの接続文字列>"
   - 設定を確認するには:
	 dotnet user-secrets list

3. 接続文字列の例（LocalDB を使う場合）
   Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\DB\\Sample.mdf;Integrated Security=True;Connect Timeout=30

注意事項
- user-secrets は開発環境専用です。本番環境では環境変数やシークレットマネージャ（Azure Key Vault 等）を利用してください。
- appsettings.json の ConnectionStrings:DefaultConnection は空にしてあり、実行環境で必ず設定してください。

必要なら、このリポジトリに対して user-secrets 初期化を自動で行うパッチも作成します。指示ください。