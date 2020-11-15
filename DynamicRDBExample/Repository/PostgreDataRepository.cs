using DynamicRDBExample.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DynamicRDBExample.Repository
{
	class PostgreDataRepository: IDataRepository
	{

		public bool IsExistTable(string tableName)
		{
			using (NpgsqlConnection conn = new PostgreDBConfig().OpendNpgsqlConnection())
			{
				string cmdStr = string.Format("SELECT * FROM information_schema.tables WHERE table_name = '{0}';", tableName);
				var cmd = new NpgsqlCommand(cmdStr, conn);
				var da = new NpgsqlDataAdapter(cmd);
				var dt = new DataTable();
				da.Fill(dt);

				if (dt.Rows.Count == 0)
					return false;
				else
					return true;
			}
		}

		public TableDefinition GetTableDefinition(string tableName)
		{
			List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
			using (NpgsqlConnection conn = new PostgreDBConfig().OpendNpgsqlConnection())
			{
				string cmdStr = string.Format("SELECT column_name ,data_type FROM information_schema.columns WHERE table_name = '{0}';", tableName);
				var cmd = new NpgsqlCommand(cmdStr, conn);
				var da = new NpgsqlDataAdapter(cmd);
				var dt = new DataTable();
				da.Fill(dt);

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					columnDefinitions.Add(new ColumnDefinition(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString()));
				}

				return new TableDefinition(tableName, columnDefinitions);
			}
		}

		public void ExecuteSql(string sql)
		{
			using (NpgsqlConnection conn = new PostgreDBConfig().OpendNpgsqlConnection())
			{
				var cmd = new NpgsqlCommand(sql, conn);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
