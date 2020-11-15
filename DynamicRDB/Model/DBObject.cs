using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDB.Model
{
	public class DBObject
	{
		public DBObject(string columnName, DBValueType valueType, string value)
		{
			this.ColumnName = columnName;
			this.ValueType = valueType;
			this.Value = value;
		}
		public string ColumnName { get; private set; }
		public DBValueType ValueType { get; private set; }
		public string Value { get; private set; }
	}
}
