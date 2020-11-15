using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBExample.Model
{
	public class TableDefinition
	{

		public TableDefinition(string tableName, List<ColumnDefinition> columnDefinitions)
		{
			this.TableName = tableName;
			this.ColumnDefinitions = columnDefinitions;
		}

		public string TableName { get; private set; }

		public List<ColumnDefinition> ColumnDefinitions { get; private set; }
	}
}
