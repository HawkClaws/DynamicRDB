using DynamicRDBFacade.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace DynamicRDBFacade.Repository
{
	public class SqliteRepository : IDataRepository
	{
		public SqliteRepository(SQLiteConnection conn)
		{
			sQLiteConnection = conn;
		}
		private SQLiteConnection sQLiteConnection;
		public void ExecuteSql(string sql)
		{
			using (SQLiteConnection conn = new SQLiteConnection(sQLiteConnection))
			{
				conn.Open();
				var cmd = new SQLiteCommand(sql, conn);
				cmd.ExecuteNonQuery();
			}
		}

		public TableDefinition GetTableDefinition(string tableName)
		{
			List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
			using (SQLiteConnection conn = new SQLiteConnection(sQLiteConnection))
			{
				conn.Open();
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
