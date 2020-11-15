using DynamicRDBFacade.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Repository
{
	public interface IDataRepository
	{
		public TableDefinition GetTableDefinition(string tableName);
		public void ExecuteSql(string sql);
	}
}
