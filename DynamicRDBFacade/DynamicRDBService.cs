using DynamicRDB.Model;
using DynamicRDB.SqlCreator;
using DynamicRDBFacade.Repository;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DynamicRDBFacade
{
	public class DynamicRDBService
	{
		private ISqlCreator SqlCreator;
		private IDataRepository DataRepository;

		public DynamicRDBService(ISqlCreator sqlCreator, IDataRepository dataRepository)
		{
			this.SqlCreator = sqlCreator;
			this.DataRepository = dataRepository;
		}

		/// <summary>
		/// 動的にテーブルとカラムを作りつつInsertを行います
		/// </summary>
		/// <param name="dbobjects"></param>
		/// <param name="tableName"></param>
		public void DynamicInsert(IEnumerable<DBObject> dbobjects, string tableName)
		{
			DynamicInsertCommon(dbobjects, tableName);
			var dml = this.SqlCreator.InsertSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		/// <summary>
		/// Insertを行います
		/// </summary>
		/// <param name="dbobjects"></param>
		/// <param name="tableName"></param>
		public void Insert(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.InsertSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		/// <summary>
		/// Updateを行います
		/// </summary>
		/// <param name="dBObjects"></param>
		/// <param name="tableName"></param>
		/// <param name="whereObj"></param>
		public void Update(IEnumerable<DBObject> dBObjects, string tableName, DBObject whereObj)
		{
			var dml = this.SqlCreator.UpdateSql(dBObjects, tableName, whereObj);
			this.DataRepository.ExecuteSql(dml);
		}

		/// <summary>
		/// 動的にテーブルとカラムを作りつつ複数行Insertを行います
		/// </summary>
		/// <param name="dbobjectsList"></param>
		/// <param name="tableName"></param>
		public void DynamicMultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjectsList, string tableName)
		{
			DynamicInsertCommon(dbobjectsList.First(), tableName);
			MultiInsert(dbobjectsList, tableName);
		}

		/// <summary>
		/// 複数行Insertを行います
		/// </summary>
		/// <param name="dbobjects"></param>
		/// <param name="tableName"></param>
		public void MultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.MultiInsert(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		private void DynamicInsertCommon(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var createTableSql = this.SqlCreator.CreateTableSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(createTableSql);

			var existsColumnsName = this.DataRepository.GetTableDefinition(tableName).ColumnDefinitions.Select(p => p.ColumnName.ToLower());
			var createColumnsName = dbobjects.Where(p => existsColumnsName.Contains(p.ColumnName.ToLower()) == false);
			if (createColumnsName.Any())
			{
				var createColumnSql = this.SqlCreator.AlterTableSql(createColumnsName, tableName);
				this.DataRepository.ExecuteSql(createColumnSql);
			}
		}
	}
}
