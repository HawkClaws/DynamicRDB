using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Model
{
	public class TableDefinition
	{

		/// <summary>
		/// DBのテーブル定義情報
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnDefinitions"></param>
		public TableDefinition(string tableName, List<ColumnDefinition> columnDefinitions)
		{
			this.TableName = tableName;
			this.ColumnDefinitions = columnDefinitions;
		}

		/// <summary>
		/// テーブル名
		/// </summary>
		public string TableName { get; }

		/// <summary>
		/// カラム情報
		/// </summary>
		public List<ColumnDefinition> ColumnDefinitions { get; }
	}
}
