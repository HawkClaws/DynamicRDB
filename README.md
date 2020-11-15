Json,Xmlを指定すると自動でRDBのテーブル、カラムを作成しInsertを行います。
DynamicInsertでは、Insertするたびに足りないカラムが自動作成されます。

現在対応しているDB
・SQLite
・PostgreSQL


複数のDBをサポートしたいが、このプロジェクト内部でConnectionを使おうとすると
ライブラリをDBと同じ数追加しないといけないので、依存関係が強く、重くなってしまう
どうにか外側からConnectionを持ってこられるようにしたい
→GenericやDynamicはあんまり使いたくない（何でも入れられちゃうので）

このプロジェクトはSQLを作成するのみで、実行は無し・・・？
→これだけだとどういう流れで使うのか分かりづらそう

実際Connectionを使ってSQLを実行する部分は外側に持っていく
→そのプロジェクトをライブラリとするかexampleとするか
→ひとまずexampleで！

テーブル定義取得と、テーブル存在チェックを一緒にしたい
カラム数でチェックしようとしたが、カラムが存在しないテーブルを作ることも可能なのでざるチェックになってしまう・・・・
→CREATE TABLE IF NOT EXISTSで解決。テーブルが存在しない場合にのみ作成する

カラムが存在しない場合に限り、ALTER TABLE文を実行してSampleColumnカラムを追加するということもしようとしたが。。。
→内部にTableDefinitionを持って、DBに接続せずカラム存在チェックをしようとしているため一旦保留
→カラムごとにチェックして〜というSQLを流すと重そうなため

課題：
SQLは大文字小文字区別されないため、存在チェックでエラーになる場合がある
SQLインジェクションはめちゃくちゃ出来る状態
Jsonの入れ子は対応していない。再帰処理使えば出来ると思うがあまりやる気が起きない
内部にTableDefinitionのリストを持って、DBに接続せずカラム存在チェックをしよう

If Json and Xml are specified, RDB tables and columns will be created automatically and Insert will be performed.
DynamicInsert automatically creates the missing columns each time you insert.

Currently supported DB
・ SQLite
・ PostgreSQL

I want to support multiple DBs, but when I try to use Connection inside this project
Since you have to add the same number of libraries as DB, the dependency is strong and it becomes heavy.
I want to be able to bring Connection from the outside somehow
→ I don't want to use Generic or Dynamic so much (because I can put anything in it)

This project only creates SQL, no execution ...?
→ It seems difficult to understand how to use it with just this

The part that actually executes SQL using Connection is brought to the outside
→ Whether the project is a library or an example
→ For the time being, use example!

I want to get the table definition and check the existence of the table together.
I tried to check by the number of columns, but it is possible to create a table without columns, so it will be a check ...
→ Solved with CREATE TABLE IF NOT EXISTS. Create only if the table does not exist

I also tried to add a SampleColumn column by executing an ALTER TABLE statement only if the column doesn't exist. .. ..
→ It has a TableDefinition inside and is temporarily suspended because it is trying to check the existence of columns without connecting to the DB.
→ Because it seems heavy if you check each column and run the SQL

Task:
SQL is not case sensitive and may result in an error in existence checking
SQL injection can be messed up
Json nesting is not supported. I think it can be done by using recursive processing, but I don't get much motivation
Let's have a list of TableDefinition inside and check column existence without connecting to DB