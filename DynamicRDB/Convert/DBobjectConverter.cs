using DynamicRDB.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DynamicRDB.Convert
{
	public class DBobjectConverter
	{
		public IEnumerable<DBObject> JsonToDBObject(JObject jObject)
		{
			List<DBObject> dBObjectList = new List<DBObject>();

			foreach (JProperty jProperty in jObject.Children())
			{
				dBObjectList.Add(new DBObject(jProperty.Name, JsonColumnTypeDifin[jProperty.Value.Type], jProperty.Value.ToString()));
			}

			return dBObjectList;
		}

		public IEnumerable<DBObject> ClassToDBObject<T>(T valueClass)
		{
			List<DBObject> dBObjectList = new List<DBObject>();

			var properties = valueClass.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				var type = propertyInfo.PropertyType;
				dBObjectList.Add(new DBObject(propertyInfo.Name, ClassColumnTypeDifin[propertyInfo.PropertyType], propertyInfo.GetValue(valueClass)?.ToString()));
			}

			return dBObjectList;
		}


		private Dictionary<JTokenType, DBValueType> JsonColumnTypeDifin = new Dictionary<JTokenType, DBValueType>()
		{
			{JTokenType.Object ,DBValueType.Object},
			{JTokenType.Array ,DBValueType.Array},
			{JTokenType.Integer ,DBValueType.Integer},
			{JTokenType.Float ,DBValueType.Double},
			{JTokenType.String,DBValueType.String},
			{JTokenType.Boolean ,DBValueType.Bool},
			{JTokenType.Date ,DBValueType.DateTime},
		};


		private Dictionary<Type, DBValueType> ClassColumnTypeDifin = new Dictionary<Type, DBValueType>()
		{
			{typeof(bool) ,DBValueType.Bool},
			{typeof(char) ,DBValueType.String},
			{typeof(string) ,DBValueType.String},
			{typeof(short) ,DBValueType.Integer},
			{typeof(int) ,DBValueType.Integer},
			{typeof(long) ,DBValueType.Integer},
			{typeof(float) ,DBValueType.Double},
			{typeof(double) ,DBValueType.Double},
			{typeof(decimal) ,DBValueType.Double},
			{typeof(DateTime) ,DBValueType.DateTime},
		};
	}
}
