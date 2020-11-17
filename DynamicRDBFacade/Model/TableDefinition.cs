using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Model
{
	public class TableDefinition
	{

		public TableDefinition(string tableName, List<ColumnDefinition> columnDefinitions)
		{
			this.TableName = tableName;
			this.ColumnDefinitions = columnDefinitions;
		}

		public string TableName { get; }

		public List<ColumnDefinition> ColumnDefinitions { get; }
	}
}
