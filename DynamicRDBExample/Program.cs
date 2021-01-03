using DynamicRDB.Convert;
using DynamicRDB.Model;
using DynamicRDB.SqlCreator;
using DynamicRDBFacade;
using DynamicRDBFacade.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DynamicRDBExample
{
	class Program
	{
		static void Main(string[] args)
		{
			var executer = new DynamicRDBService(new SQLiteCreator(), new SqliteRepository(new SqliteDBConfig().SQLiteConnection()));
			//var executer = new DynamicRDBService(new PostgreCreator(), new PostgreRepository(new PostgreDBConfig().NpgsqlConnection()));

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
			foreach (JObject j in jObject["array"].Children())
			{
				var dbinfo = CreateInfo(j);
				dBObjects.Add(dbinfo.Item1);
				dbName = dbinfo.Item2;
			}
			executer.DynamicMultiInsert(dBObjects, dbName);

			//dataclass(便利なRapper)
			var productData = new ProductData { ProductName = "Magazine", Price = 500, ReleaseDate = DateTime.Now };
			new CommonRepository(new SQLiteCreator(), new SqliteRepository(new SqliteDBConfig().SQLiteConnection())).Upsert(productData, nameof(ProductData.ProductName));
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

	internal class ProductData
	{
		public DateTime ReleaseDate { get; set; }
		public string ProductName { get; set; }
		public long Price { get; set; }
	}
}
