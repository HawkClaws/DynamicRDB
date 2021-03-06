﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Model
{
	public class ColumnDefinition
	{
		/// <summary>
		/// カラム名
		/// </summary>
		public string ColumnName { get; }

		/// <summary>
		/// カラムのデータタイプ
		/// </summary>
		public string DataType { get; }

		public ColumnDefinition(string columnName, string dataType)
		{
			this.ColumnName = columnName;
			this.DataType = dataType;
		}
	}
}
