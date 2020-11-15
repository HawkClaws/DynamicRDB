using DynamicRDB.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
				dBObjectList.Add(new DBObject(jProperty.Name, ColumnTypeDifin[jProperty.Value.Type], jProperty.Value.ToString()));
			}

			return dBObjectList;
		}


		private Dictionary<JTokenType, DBValueType> ColumnTypeDifin = new Dictionary<JTokenType, DBValueType>()
		{
			{JTokenType.Object ,DBValueType.Object},
			{JTokenType.Array ,DBValueType.Array},
			{JTokenType.Integer ,DBValueType.Integer},
			{JTokenType.Float ,DBValueType.Double},
			{JTokenType.String,DBValueType.String},
			{JTokenType.Boolean ,DBValueType.Bool},
			{JTokenType.Date ,DBValueType.DateTime},
		};
	}
}
