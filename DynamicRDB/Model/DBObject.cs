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
		public string ColumnName { get; }
		public DBValueType ValueType { get; }
		public string Value { get; }
		public bool Unique { get; set; }
	}
}
