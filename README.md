Json,Xml���w�肷��Ǝ�����RDB�̃e�[�u���A�J�������쐬��Insert���s���܂��B
DynamicInsert�ł́AInsert���邽�тɑ���Ȃ��J�����������쐬����܂��B

���ݑΉ����Ă���DB
�ESQLite
�EPostgreSQL


������DB���T�|�[�g���������A���̃v���W�F�N�g������Connection���g�����Ƃ����
���C�u������DB�Ɠ������ǉ����Ȃ��Ƃ����Ȃ��̂ŁA�ˑ��֌W�������A�d���Ȃ��Ă��܂�
�ǂ��ɂ��O������Connection�������Ă�����悤�ɂ�����
��Generic��Dynamic�͂���܂�g�������Ȃ��i���ł������ꂿ�Ⴄ�̂Łj

���̃v���W�F�N�g��SQL���쐬����݂̂ŁA���s�͖����E�E�E�H
�����ꂾ�����Ƃǂ���������Ŏg���̂�������Â炻��

����Connection���g����SQL�����s���镔���͊O���Ɏ����Ă���
�����̃v���W�F�N�g�����C�u�����Ƃ��邩example�Ƃ��邩
���ЂƂ܂�example�ŁI

�e�[�u����`�擾�ƁA�e�[�u�����݃`�F�b�N���ꏏ�ɂ�����
�J�������Ń`�F�b�N���悤�Ƃ������A�J���������݂��Ȃ��e�[�u������邱�Ƃ��\�Ȃ̂ł���`�F�b�N�ɂȂ��Ă��܂��E�E�E�E
��CREATE TABLE IF NOT EXISTS�ŉ����B�e�[�u�������݂��Ȃ��ꍇ�ɂ̂ݍ쐬����

�J���������݂��Ȃ��ꍇ�Ɍ���AALTER TABLE�������s����SampleColumn�J������ǉ�����Ƃ������Ƃ����悤�Ƃ������B�B�B
��������TableDefinition�������āADB�ɐڑ������J�������݃`�F�b�N�����悤�Ƃ��Ă��邽�߈�U�ۗ�
���J�������ƂɃ`�F�b�N���ā`�Ƃ���SQL�𗬂��Əd�����Ȃ���

�ۑ�F
SQL�͑啶����������ʂ���Ȃ����߁A���݃`�F�b�N�ŃG���[�ɂȂ�ꍇ������
SQL�C���W�F�N�V�����͂߂��Ⴍ����o������
Json�̓���q�͑Ή����Ă��Ȃ��B�ċA�����g���Ώo����Ǝv�������܂���C���N���Ȃ�
������TableDefinition�̃��X�g�������āADB�ɐڑ������J�������݃`�F�b�N�����悤

If Json and Xml are specified, RDB tables and columns will be created automatically and Insert will be performed.
DynamicInsert automatically creates the missing columns each time you insert.

Currently supported DB
�E SQLite
�E PostgreSQL

I want to support multiple DBs, but when I try to use Connection inside this project
Since you have to add the same number of libraries as DB, the dependency is strong and it becomes heavy.
I want to be able to bring Connection from the outside somehow
�� I don't want to use Generic or Dynamic so much (because I can put anything in it)

This project only creates SQL, no execution ...?
�� It seems difficult to understand how to use it with just this

The part that actually executes SQL using Connection is brought to the outside
�� Whether the project is a library or an example
�� For the time being, use example!

I want to get the table definition and check the existence of the table together.
I tried to check by the number of columns, but it is possible to create a table without columns, so it will be a check ...
�� Solved with CREATE TABLE IF NOT EXISTS. Create only if the table does not exist

I also tried to add a SampleColumn column by executing an ALTER TABLE statement only if the column doesn't exist. .. ..
�� It has a TableDefinition inside and is temporarily suspended because it is trying to check the existence of columns without connecting to the DB.
�� Because it seems heavy if you check each column and run the SQL

Task:
SQL is not case sensitive and may result in an error in existence checking
SQL injection can be messed up
Json nesting is not supported. I think it can be done by using recursive processing, but I don't get much motivation
Let's have a list of TableDefinition inside and check column existence without connecting to DB