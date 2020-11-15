using DynamicRDBExample.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace DynamicRDBExample.Repository
{
	class SqliteRepository : IDataRepository
	{
		public void ExecuteSql(string sql)
		{
			using (SQLiteConnection conn = new SqliteDBConfig().OpendSQLiteConnection())
			{
				var cmd = new SQLiteCommand(sql, conn);
				cmd.ExecuteNonQuery();
			}
		}

		public TableDefinition GetTableDefinition(string tableName)
		{
			List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
			using (SQLiteConnection conn = new SqliteDBConfig().OpendSQLiteConnection())
			{
				string cmdStr = string.Format("PRAGMA table_info('{0}');", tableName);
				var cmd = new SQLiteCommand(cmdStr, conn);
				var da = new SQLiteDataAdapter(cmd);
				var dt = new DataTable();
				da.Fill(dt);

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					columnDefinitions.Add(new ColumnDefinition(dt.Rows[i]["name"].ToString(), dt.Rows[i]["type"].ToString()));
				}

				return new TableDefinition(tableName, columnDefinitions);
			}
		}
	}
}
