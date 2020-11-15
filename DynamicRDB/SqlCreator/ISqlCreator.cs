using DynamicRDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDB.SqlCreator
{
	public interface ISqlCreator
	{
		public string CreateTableSql(IEnumerable<DBObject> dBObjects, string tableName);
		public string AlterTableSql(IEnumerable<DBObject> dBObjects, string tableName);
		public string InsertSql(IEnumerable<DBObject> dBObjects, string tableName);
		public string MultiInsert(IEnumerable<IEnumerable<DBObject>> dBObjectsList, string tableName);

	}
}
