using Dapper;
using DynamicRDB.Convert;
using DynamicRDB.SqlCreator;
using DynamicRDBExample.Repository;
using DynamicRDBFacade;
using DynamicRDBFacade.Repository;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace DynamicRDBExample
{
	public class CommonRepository
	{
		private ISqlCreator SqlCreator;
		private IDataRepository DataRepository;
		public CommonRepository(ISqlCreator sqlCreator, IDataRepository dataRepository)
		{
			this.SqlCreator = sqlCreator;
			this.DataRepository = dataRepository;
		}
	

		public IEnumerable<T> Query<T>() where T : new()
		{
			var data = new T();
			var dbobjects = new DBobjectConverter().ClassToDBObject(data);

			var sql = this.SqlCreator.SelectSql(dbobjects, typeof(T).Name);
			using (IDbConnection conn = new PostgreDBConfig().NpgsqlConnection())
			{
				conn.Open();
				return conn.Query<T>(sql);
			}
		}

		public void Upsert<T>(T dataClass, string uniqueColmn)
		{
			var executer = new DynamicRDBService(this.SqlCreator, this.DataRepository);
			var dbobjects = new DBobjectConverter().ClassToDBObject(dataClass);
			dbobjects.Where(p => p.ColumnName == uniqueColmn.ToLower()).First().Unique = true;
			try
			{
				executer.DynamicInsert(dbobjects, dataClass.GetType().Name.ToLower());
			}
			catch (DbException ex)
			{
				//本当はもっと厳密にチェックしなくては。。。			
				executer.Update(dbobjects, dataClass.GetType().Name.ToLower(), dbobjects.Where(p => p.Unique == true).FirstOrDefault());
			}
		}
	}
}