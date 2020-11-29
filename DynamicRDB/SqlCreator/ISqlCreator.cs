using DynamicRDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDB.SqlCreator
{
	public interface ISqlCreator
	{
		/// <summary>
		/// DBテーブルを作成するSQLを発行します
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public string CreateTableSql(IEnumerable<DBObject> dBObjects, string tableName);

		/// <summary>
		/// DBテーブルの不足分のカラムを作成するSQLを発行します
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public string AlterTableSql(IEnumerable<DBObject> dBObjects, string tableName);

		/// <summary>
		/// InsertするSQLを発行します
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public string InsertSql(IEnumerable<DBObject> dBObjects, string tableName);

		/// <summary>
		/// UpdateするSQLを発行します
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <param name="whereObj"></param>
		/// <returns></returns>
		public string UpdateSql(IEnumerable<DBObject> dBObjects, string tableName, DBObject whereObj);

		/// <summary>
		/// MultiInsertするSQLを発行します
		/// </summary>
		/// <param name="dBObjectsList"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public string MultiInsert(IEnumerable<IEnumerable<DBObject>> dBObjectsList, string tableName);


		/// <summary>
		/// SelectするSQLを発行します
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <param name="whereObj"></param>
		/// <returns></returns>
		public string SelectSql(IEnumerable<DBObject> dBObjects, string tableName, DBObject whereObj = null);
	}
}
