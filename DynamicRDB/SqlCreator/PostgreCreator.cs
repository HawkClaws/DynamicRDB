using DynamicRDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDB.SqlCreator
{
	public class PostgreCreator : ISqlCreator
	{
		public string CreateTableSql(IEnumerable<DBObject> dBObjects, string tableName)
		{
			List<string> difinList = new List<string>();

			foreach (DBObject dBObject in dBObjects)
			{
				difinList.Add(string.Format("{0} {1}", dBObject.ColumnName, ColumnTypeDifin[dBObject.ValueType]));
			}

			string cmdStrBase = @"CREATE TABLE IF NOT EXISTS {0} ({1});";

			return string.Format(cmdStrBase, tableName, string.Join(',', difinList));
		}

		public string AlterTableSql(IEnumerable<DBObject> dBObjects, string tableName)
		{
			List<string> difinList = new List<string>();

			foreach (DBObject dBObject in dBObjects)
			{
				difinList.Add(string.Format("ADD COLUMN {0} {1}", dBObject.ColumnName, ColumnTypeDifin[dBObject.ValueType]));
			}

			string cmdStrBase = @"ALTER TABLE {0} {1};";

			return string.Format(cmdStrBase, tableName, string.Join(',', difinList));
		}

		public string InsertSql(IEnumerable<DBObject> dBObjects, string tableName)
		{
			List<string> columnList = new List<string>();
			List<string> valueList = new List<string>();

			foreach (DBObject dBObject in dBObjects)
			{
				valueList.Add(string.Format(ValueTypeDifin[dBObject.ValueType], dBObject.Value));
				columnList.Add(dBObject.ColumnName);
			}

			string cmdStrBase = @"INSERT INTO {0} ({1}) VALUES ({2});";

			return string.Format(cmdStrBase, tableName, string.Join(',', columnList), string.Join(',', valueList));
		}


		//public void MultiInsert(string tableName, long[] datas)
		//{
		//	using (NpgsqlConnection conn = DBConfig.OpendNpgsqlConnection())
		//	{
		//		string valuesBase = "( {0}, '{0}' )";
		//		string cmdStrBase = @"INSERT INTO {0} (id, name) VALUES {1};";
		//		string values = string.Join(",", datas.Select(p => string.Format(valuesBase, p)));

		//		string cmdStr = string.Format(cmdStrBase, tableName, values);
		//		var cmd = new NpgsqlCommand(cmdStr, conn);
		//		cmd.ExecuteNonQuery();
		//	}
		//}

		private Dictionary<DBValueType, string> ColumnTypeDifin = new Dictionary<DBValueType, string>()
		{
			{DBValueType.Object ,"json"},
			{DBValueType.Array ,"json"},
			{DBValueType.Integer ,"bigint"},
			{DBValueType.Double ,"double precision"},
			{DBValueType.String,"text" },
			{DBValueType.Bool ,"boolean"},
			{DBValueType.DateTime ,"date"},
		};

		private Dictionary<DBValueType, string> ValueTypeDifin = new Dictionary<DBValueType, string>()
		{
			{DBValueType.Object ,"'{0}'"},
			{DBValueType.Array ,"'{0}'"},
			{DBValueType.Integer ,"{0}"},
			{DBValueType.Double ,"{0}"},
			{DBValueType.String,"'{0}'" },
			{DBValueType.Bool ,"{0}"},
			{DBValueType.DateTime ,"'{0}'"},
		};
	}
}
