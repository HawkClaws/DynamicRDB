using DynamicRDBExample.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBExample.Repository
{
	interface IDataRepository
	{
		public TableDefinition GetTableDefinition(string tableName);
		public void ExecuteSql(string sql);
	}
}
