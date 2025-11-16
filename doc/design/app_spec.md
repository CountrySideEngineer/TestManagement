# テスト管理アプリケーション 仕様書（Draft v0.9）

## 1. 概要

本アプリケーションは、単体テスト・結合テスト・システムテストなどの実行を管理する Web アプリケーション（Razor Pages + Web API）である。  
テスト資産（TestSuite, TestCase）を管理し、テスト実行情報（TestRun, TestResult）を記録・検索・閲覧できることを目的とする。

利用環境としては、主に個人開発または小規模チームを想定する。

---

## 2. 主な利用目的

- テストケースを体系的に管理する。
- テスト実行の記録（誰が、いつ、どの環境で実施したか）を残す。
- テスト結果を定量的に追跡する。
- 過去のテスト履歴の参照や集計を行う。

---

## 3. 利用者（ロール）

| ロール | 説明 |
|-------|------|
| Tester（テスター） | テストを実行し TestRun を登録する |
| Developer（開発者） | TestCase／TestSuite のメンテナンスを行う |
| Admin（管理者） | 全データの管理 |

現時点では認証／認可機能は未実装であり、将来追加予定。

---

## 4. ドメインモデル

### 4.1 Project

テスト対象となるプロダクトまたはサブシステム。

| プロパティ | 型 | 説明 |
|------------|----|------|
| Id | int | PK |
| Name | string | プロジェクト名 |
| Description | string? | 説明文 |
| TestSuites | ICollection<TestSuite> | 配下のテストスイート |

---

### 4.2 TestSuite

関連する TestCase のまとまり（機能単位など）。

| プロパティ | 型 | 説明 |
|------------|----|------|
| Id | int | PK |
| ProjectId | int | 親 Project |
| Name | string | テストスイート名 |
| TestCases | ICollection<TestCase> | 配下のテストケース |

---

### 4.3 TestCase

個別のテストケース（観点／仕様の1項目）。

| プロパティ | 型 |
|------------|----|
| Id | int |
| TestSuiteId | int |
| Title | string |
| ExpectedResult | string? |
| Steps | string? |
| TestResults | ICollection<TestResult> |

---

### 4.4 TestRun

テストを 1 回実行した際の情報。

| プロパティ | 型 | 説明 |
|------------|----|------|
| Id | int | PK |
| ProjectId | int | どのプロジェクトのテスト実行か |
| TesterId | int? | 実行者（任意） |
| ExecutedAt | DateTimeOffset | テスト実施日時（ユーザが指定する） |
| Environment | string? | 実行環境情報（OS, バージョン, CI情報 など） |
| Notes | string? | メモ |
| TestResults | ICollection<TestResult> | 実行した TestCase の結果一覧 |

---

### 4.5 TestResult

TestRun 内の TestCase の結果情報。

| プロパティ | 型 | 説明 |
|------------|----|------|
| Id | int | PK |
| TestRunId | int | 親 TestRun |
| TestCaseId | int | 対象 TestCase |
| Status | enum(Passed/Failed/Blocked/Skipped) | 結果 |
| ActualResult | string? | 実際の結果 |
| DurationMs | int? | 所要時間 |
| Comment | string? | 備考 |

---

### 4.6 Tester

テスト担当者（任意機能）。

| プロパティ | 型 |
|------------|----|
| Id | int |
| Name | string |
| Email | string? |

---

## 5. 機能要件

### 5.1 管理機能

- Project の登録・更新・削除
- TestSuite の登録・更新・削除
- TestCase の登録・更新・削除
- Tester の管理

### 5.2 テスト実行と結果記録

- TestRun + TestResults の一括登録  
  - TestRun（日時、環境、メモ）  
  - TestResults（複数件）

### 5.3 検索・参照機能

- プロジェクト別 TestSuite／TestCase の閲覧
- 最新 TestRun の一覧
- 任意期間の TestRun 検索
- TestRun → TestResults の詳細表示

### 5.4 将来拡張予定

- グラフ表示（合格率推移など）
- 統計 API の追加
- CSV/Excelエクスポート

---

## 6. 非機能要件

- フレームワーク: .NET 8 ASP.NET Core  
- ORM: Entity Framework Core (PostgreSQL Provider)  
- DB: PostgreSQL  
- API: RESTful JSON API  
- ロギング: ASP.NET Core 標準機能  
- Migration: EF Core Migrations を使用  

---

## 7. API 仕様（暫定）

### 7.1 Project API

| メソッド | URL                | 説明     |
|---------|--------------------|----------|
| GET     | /api/projects      | 一覧取得 |
| GET     | /api/projects/{id} | 詳細取得 |
| POST    | /api/projects      | 作成     |
| PUT     | /api/projects/{id} | 更新     |
| DELETE  | /api/projects/{id} | 削除     |

### 7.2 TestSuite API

| メソッド   | URL                                  | 説明    |
|-----------| ------------------------------------ | ------- |
| GET       | /api/projects/{projectId}/testsuites | 一覧取得 |
| GET       | /api/testsuites/{id}                 | 詳細取得 |
| POST      | /api/projects/{projectId}/testsuites | 作成     |
| PUT       | /api/testsuites/{id}                 | 更新     |
| DELETE    | /api/testsuites/{id}                 | 削除     |

### 7.3 TestCase API

| メソッド   | URL                                 | 説明   |
| ------ | ----------------------------------- | ---- |
| GET    | /api/testsuites/{suiteId}/testcases | 一覧取得 |
| GET    | /api/testcases/{id}                 | 詳細取得 |
| POST   | /api/testsuites/{suiteId}/testcases | 作成   |
| PUT    | /api/testcases/{id}                 | 更新   |
| DELETE | /api/testcases/{id}                 | 削除   |

### 7.4 Tester API

| メソッド   | URL               | 説明     |
| --------- | ----------------- | -------- |
| GET       | /api/testers      | 一覧取得 |
| GET       | /api/testers/{id} | 詳細取得 |
| POST      | /api/testers      | 作成     |
| PUT       | /api/testers/{id} | 更新     |
| DELETE    | /api/testers/{id} | 削除     |


### 7.5 TestRun 一括登録 API

#### 1) 通常CRUD

| メソッド   | URL                | 説明                |
| --------- | ------------------ | ------------------- |
| GET       | /api/testruns      | 一覧                |
| GET       | /api/testruns/{id} | 詳細                |
| POST      | /api/testruns      | 作成（TestRun 単体） |
| PUT       | /api/testruns/{id} | 更新                |
| DELETE    | /api/testruns/{id} | 削除                |

#### 2）TestRun + TestResults まとめ登録 API

| メソッド | URL                        | 説明                             |
| ------- | -------------------------- | ------------------------------- |
| POST    | /api/testruns/with-results | TestRun + TestResults を一括登録 |

#### リクエスト例

```json
{
  "projectId": 1,
  "testerId": 3,
  "executedAt": "2025-02-15T15:30:00+09:00",
  "environment": "windows-11 .NET 8",
  "notes": "リリース前最終テスト",
  "results": [
    {
      "testCaseId": 10,
      "status": "Passed",
      "actualResult": "期待通り動作",
      "durationMs": 1300
    },
    {
      "testCaseId": 11,
      "status": "Failed",
      "actualResult": "例外発生",
      "comment": "ログ参照必要"
    }
  ]
}
```

## 8. データベース構造（Enum 省略）

### 8.1 テーブル一覧

* Projects
* TestSuites
* TestCases
* TestRuns
* TestResults
* Testers

## 9. 制約・前提

* ExecutedAt はユーザが明示的に指定する
* TestRun と TestResults はビジネス的に一体であり、単独登録は行わない
* TestRun → TestResults → TestCase の構造で多対多を表現する
* PostgreSQL の命名規則に従い、テーブル名は複数形

