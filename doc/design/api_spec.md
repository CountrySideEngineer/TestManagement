# API Specification (Draft)

## 1. TestLevel API

| method   | URL            | description   |
| -------- | -------------- | ------------- |
| GET      | /api/testlevel | 一覧取得      |

## 2. TestCase API

| method   | URL                | description   |
| -------- | ------------------ | ------------- |
| GET      | /api/testcase/     | 一覧取得      |
| GET      | /api/testcase/{id} | 詳細取得      |
| POST     | /api/testcase/     | 作成          |
| POST     | /api/testcase/bulk | 作成(一括)    |

## 3. TestRun API

| method   | URL                | description   |
| -------- | ------------------ | ------------- |
| GET      | /api/testrun/      | 一覧取得       |
| GET      | /api/testrun/{id}  | 詳細取得       |
| POST     | /api/testrun/      | 作成          |
| POST     | /api/testrun/bulk  | 作成(一括)    |

## 4. TestResult API

| method   | URL                   | description   |
| -------- | --------------------- | ------------- |
| GET      | /api/testresult/      | 一覧取得      |
| GET      | /api/testresult/{id}  | 詳細取得      |
| POST     | /api/testresult/      | 作成          |
| POST     | /api/testresult/bulk  | 作成(一括)    |
