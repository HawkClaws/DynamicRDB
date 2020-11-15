using DynamicRDB.Model;
using DynamicRDB.SqlCreator;
using DynamicRDBFacade.Repository;
using System;
using System.Collections.Generic;
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

		public void DynamicInsert(IEnumerable<DBObject> dbobjects, string tableName)
		{
			CreateTable(dbobjects, tableName);

			var existsColumnsName = this.DataRepository.GetTableDefinition(tableName).ColumnDefinitions.Select(p => p.ColumnName);
			var createColumnsName = dbobjects.Where(p => existsColumnsName.Contains(p.ColumnName) == false);
			if (createColumnsName.Any())
			{
				var createColumnSql = this.SqlCreator.AlterTableSql(createColumnsName, tableName);
				this.DataRepository.ExecuteSql(createColumnSql);
			}

			StaticInsert(dbobjects, tableName);
		}

		public void StaticInsert(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.InsertSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		public void DynamicMultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjectsList, string tableName)
		{
			CreateTable(dbobjectsList.First(), tableName);

			var existsColumnsName = this.DataRepository.GetTableDefinition(tableName).ColumnDefinitions.Select(p => p.ColumnName);
			var createColumnsName = dbobjectsList.First().Where(p => existsColumnsName.Contains(p.ColumnName) == false);
			if (createColumnsName.Any())
			{
				var createColumnSql = this.SqlCreator.AlterTableSql(createColumnsName, tableName);
				this.DataRepository.ExecuteSql(createColumnSql);
			}

			StaticMultiInsert(dbobjectsList, tableName);
		}
		public void StaticMultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.MultiInsert(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		public void CreateTable(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var createTableSql = this.SqlCreator.CreateTableSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(createTableSql);
		}
	}
}
