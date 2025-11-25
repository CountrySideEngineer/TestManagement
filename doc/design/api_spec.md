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
