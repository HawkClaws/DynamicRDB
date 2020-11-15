using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Model
{
	public class ColumnDefinition
	{
		public string ColumnName { get; private set; }
		public string DataType { get; private set; }

		public ColumnDefinition(string columnName, string dataType)
		{
			this.ColumnName = columnName;
			this.DataType = dataType;
		}
	}
}
