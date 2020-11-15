using DynamicRDB.Convert;
using DynamicRDB.Model;
using DynamicRDB.SqlCreator;
using DynamicRDBExample.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace DynamicRDBExample
{
	class Program
	{
		static void Main(string[] args)
		{
			//var executer = new Executer(new SQLiteCreator(), new SqliteRepository());
			var executer = new Executer(new PostgreCreator(), new PostgreDataRepository());

			var startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); ;

			//Common
			JObject jObject = ReadJsonFile(Path.Combine(startupPath, "test.json"));
			var info = CreateInfo(jObject);
			executer.DynamicInsert(info.Item1, info.Item2);

			jObject = ReadJsonFile(Path.Combine(startupPath, "test2.json"));
			info = CreateInfo(jObject);
			executer.DynamicInsert(info.Item1, info.Item2);
			
			//AddColumn
			jObject = ReadJsonFile(Path.Combine(startupPath, "test3.json"));
			info = CreateInfo(jObject);
			executer.DynamicInsert(info.Item1, info.Item2);

			//xml
			XmlDocument doc = new XmlDocument();
			var str = ReadFile(Path.Combine(startupPath, "test4.xml"));
			doc.LoadXml(str);
			string jsonText = JsonConvert.SerializeXmlNode(doc);
			var jObjectTemp = JObject.Parse(jsonText);
			jObject = new JObject();

			foreach (var j in jObjectTemp["xml"].Children())
				jObject.Add(j);

			info = CreateInfo(jObject);
			executer.DynamicInsert(info.Item1, info.Item2);

			//Multi
			jObject = ReadJsonFile(Path.Combine(startupPath, "test5.json"));
			List<IEnumerable<DBObject>> dBObjects = new List<IEnumerable<DBObject>>();

			string dbName = string.Empty;
			foreach (JObject j in jObject["array"].Children()){
				var dbinfo = CreateInfo(j);
				dBObjects.Add(dbinfo.Item1);
				dbName = dbinfo.Item2;
			}
			executer.DynamicMultiInsert(dBObjects, dbName);

		}

		private static JObject ReadJsonFile(string path)
		{
			var jsonStr = ReadFile(path);
			return JObject.Parse(jsonStr);
		}

		private static string ReadFile(string path)
		{
			string str = string.Empty;
			using (StreamReader sr = new StreamReader(path))
			{
				str = sr.ReadToEnd();
			}
			return str;
		}

		private static (IEnumerable<DBObject>, string) CreateInfo(JObject json)
		{
			string tableName = json["category"].ToString();
			json.Remove("category");

			var dbobjects = new DBobjectConverter().JsonToDBObject(json);

			return (dbobjects, tableName);
		}
	}

	internal class Executer
	{
		private ISqlCreator SqlCreator;
		private IDataRepository DataRepository;

		internal Executer(ISqlCreator sqlCreator, IDataRepository dataRepository)
		{
			this.SqlCreator = sqlCreator;
			this.DataRepository = dataRepository;
		}

		internal void DynamicInsert(IEnumerable<DBObject> dbobjects, string tableName)
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

		internal void StaticInsert(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.InsertSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		internal void DynamicMultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjectsList, string tableName)
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
		internal void StaticMultiInsert(IEnumerable<IEnumerable<DBObject>> dbobjects, string tableName)
		{
			var dml = this.SqlCreator.MultiInsert(dbobjects, tableName);
			this.DataRepository.ExecuteSql(dml);
		}

		internal void CreateTable(IEnumerable<DBObject> dbobjects, string tableName)
		{
			var createTableSql = this.SqlCreator.CreateTableSql(dbobjects, tableName);
			this.DataRepository.ExecuteSql(createTableSql);
		}
	}
}
