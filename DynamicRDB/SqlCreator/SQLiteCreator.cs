using DynamicRDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicRDB.SqlCreator
{
	public class SQLiteCreator : ISqlCreator
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
				difinList.Add(string.Format("ALTER TABLE {0} ADD COLUMN {1} {2};", tableName, dBObject.ColumnName, ColumnTypeDifin[dBObject.ValueType]));
			}
			return string.Join(string.Empty, difinList);
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

		public string MultiInsert(IEnumerable<IEnumerable<DBObject>> dBObjectsList, string tableName)
		{
			List<string> columnList = new List<string>();
			List<string> valueList = new List<string>();


			foreach (DBObject dBObject in dBObjectsList.First())
			{
				columnList.Add(dBObject.ColumnName);
			}

			foreach (var dBObjects in dBObjectsList)
			{
				List<string> values = new List<string>();
				foreach (DBObject dBObject in dBObjects)
				{
					values.Add(string.Format(ValueTypeDifin[dBObject.ValueType], dBObject.Value));
				}
				valueList.Add('(' + string.Join(',', values) + ')');
			}
			string cmdStrBase = @"INSERT INTO {0} ({1}) VALUES {2};";

			return string.Format(cmdStrBase, tableName, string.Join(',', columnList), string.Join(',', valueList));
		}

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
