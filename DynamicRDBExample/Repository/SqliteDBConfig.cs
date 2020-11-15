using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace DynamicRDBExample.Repository
{
	public class SqliteDBConfig
	{

		public string DBName { get; } = "sqlite.db";
		public SQLiteConnection OpendSQLiteConnection()
		{
			var connectionSb = new SQLiteConnectionStringBuilder { DataSource = DBName };
			var con = new SQLiteConnection(connectionSb.ToString());
			con.Open();
			return con;
		}
	}
}
