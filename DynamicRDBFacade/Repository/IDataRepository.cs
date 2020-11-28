using DynamicRDBFacade.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicRDBFacade.Repository
{
	public interface IDataRepository
	{
		/// <summary>
		/// DynamicInsertで作られた、DBのテーブル定義情報を取得します
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public TableDefinition GetTableDefinition(string tableName);

		/// <summary>
		/// SQLを実行します
		/// </summary>
		/// <param name="sql"></param>
		public void ExecuteSql(string sql);
	}
}
