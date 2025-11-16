# テスト管理アプリケーション ロードマップ

## フェーズ 1：コンセプト設計・要件整理

* 目的・役割・ユースケースの整理
* 利用者（開発者・管理者）ダッシュボードの議論
* 表示項目の仕様策定
* 画面構成（ワイヤーフレーム）作成
* 開発者向け／管理者向けダッシュボードの機能分離
* 必要なデータ項目・データフローの確定

## フェーズ 2：データモデル設計

* 必要なモデルの洗い出し
* モデル間リレーションの避けられない論点整理（TestSuite必要性、TestCase → TestLevel の方向など）
* ER 図の複数回ブラッシュアップ
* 最終モデル構成確定

## フェーズ 3：アプリ基盤の構築

* ASP.NET Core プロジェクト作成
* EF Core のセットアップ
* PostgreSQL への接続設定
* シードデータ登録
* Migration の実行とデータベース確立
* 接続トラブルの解決（DbContextOptions の設定、Migration の抜けなど）

## フェーズ 4：リポジトリ層の実装

* リポジトリ設計方針の決定
* ProjectRepository の実装（CRD/Update）
* TestSuiteRepository の実装
* TestCaseRepository の実装
* TestRunRepository の設計（モデル修正に合わせた再設計）
* TestRun + TestResults の一括登録という重要要件への対応準備

## フェーズ 5：API 層の実装

* Project API
* TestSuite API
* TestCase API
* TestRun 一括登録 API
* TestResult API（TestRun との一体登録のため補助的）

## フェーズ 6：フロントエンド（Razor Pages）実装

* ダッシュボード UI（開発者向け・管理者向け）
* プロジェクト管理ページ
* TestSuite／TestCase 管理ページ
* TestRun 実行画面（手動登録 UI）
* テスト結果閲覧 UI

### フェーズ 7：可視化・分析機能（未着手）

* 成功率・進捗率の統計
* 折れ線グラフ・円グラフの可視化
* プロジェクト別のテスト推移表示
* フィルタリング・検索機能の拡張

### フェーズ 8：品質改善・運用準備

* 入力バリデーション強化
* ログ・例外処理の整備
* API の OpenAPI 仕様書化
* CI/CD 導入（任意）
* テスト自動化（ユニットテスト）
* 本番運用設定（データバックアップ戦略、DB migration 運用）
