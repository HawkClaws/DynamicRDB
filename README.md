Json,Xmlを指定すると自動でRDBのテーブル、カラムを作成しInsertを行います。<br>
DynamicInsertでは、Insertするたびに足りないカラムが自動作成されます。<br>
<br>
現在対応しているDB<br>
・SQLite<br>
・PostgreSQL<br>
<br>
<br>
複数のDBをサポートしたいが、このプロジェクト内部でConnectionを使おうとすると<br>
ライブラリをDBと同じ数追加しないといけないので、依存関係が強く、重くなってしまう<br>
どうにか外側からConnectionを持ってこられるようにしたい<br>
→GenericやDynamicはあんまり使いたくない（何でも入れられちゃうので）<br>
<br>
このプロジェクトはSQLを作成するのみで、実行は無し・・・？<br>
→これだけだとどういう流れで使うのか分かりづらそう<br>
<br>
実際Connectionを使ってSQLを実行する部分は外側に持っていく<br>
→そのプロジェクトをライブラリとするかexampleとするか<br>
→ひとまずexampleで！<br>
<br>
テーブル定義取得と、テーブル存在チェックを一緒にしたい<br>
カラム数でチェックしようとしたが、カラムが存在しないテーブルを作ることも可能なのでざるチェックになってしまう・・・・<br>
→CREATE TABLE IF NOT EXISTSで解決。テーブルが存在しない場合にのみ作成する<br>
<br>
カラムが存在しない場合に限り、ALTER TABLE文を実行してSampleColumnカラムを追加するということもしようとしたが。。。<br>
→内部にTableDefinitionを持って、DBに接続せずカラム存在チェックをしようとしているため一旦保留<br>
→カラムごとにチェックして～というSQLを流すと重そうなため<br>
<br>
課題：<br>
SQLは大文字小文字区別されないため、存在チェックでエラーになる場合がある<br>
SQLインジェクションはめちゃくちゃ出来る状態<br>
Jsonの入れ子は対応していない。再帰処理使えば出来ると思うがあまりやる気が起きない<br>
内部にTableDefinitionのリストを持って、DBに接続せずカラム存在チェックをしよう<br>
TableDefinitionの際、テーブル名が大文字だと取得できなかったりする？<br>
　→インサートの時にテーブル名が勝手に小文字にってる<br>
<br>
If Json and Xml are specified, RDB tables and columns will be created automatically and Insert will be performed.<br>
DynamicInsert automatically creates the missing columns each time you insert.<br>
<br>
Currently supported DB<br>
・ SQLite<br>
・ PostgreSQL<br>
<br>
I want to support multiple DBs, but when I try to use Connection inside this project<br>
Since you have to add the same number of libraries as DB, the dependency is strong and it becomes heavy.<br>
I want to be able to bring Connection from the outside somehow<br>
→ I don't want to use Generic or Dynamic so much (because I can put anything in it)<br>
<br>
This project only creates SQL, no execution ...?<br>
→ It seems difficult to understand how to use it with just this<br>
<br>
The part that actually executes SQL using Connection is brought to the outside<br>
→ Whether the project is a library or an example<br>
→ For the time being, use example!<br>
<br>
I want to get the table definition and check the existence of the table together.<br>
I tried to check by the number of columns, but it is possible to create a table without columns, so it will be a check ...<br>
→ Solved with CREATE TABLE IF NOT EXISTS. Create only if the table does not exist<br>
<br>
I also tried to add a SampleColumn column by executing an ALTER TABLE statement only if the column doesn't exist. .. ..<br>
→ It has a TableDefinition inside and is temporarily suspended because it is trying to check the existence of columns without connecting to the DB.<br>
→ Because it seems heavy if you check each column and run the SQL<br>
<br>
Task:<br>
SQL is not case sensitive and may result in an error in existence checking<br>
SQL injection can be messed up<br>
Json nesting is not supported. I think it can be done by using recursive processing, but I don't get much motivation<br>
Let's have a list of TableDefinition inside and check column existence without connecting to DB