﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DynamicRDBFacade
{
	public class PostgreDBConfig
	{
		public  NpgsqlConnection NpgsqlConnection()
		{
			var con = new NpgsqlConnection(PostgreDBConfig.ConnectionString);
			return con;
		}

		private static string ConnectionString = new NpgsqlConnectionStringBuilder
		{
			Host = ConfigurationManager.AppSettings["Host"],
			Port = 5432,
			Username = ConfigurationManager.AppSettings["Username"],
			Password = ConfigurationManager.AppSettings["Password"],
			Database = ConfigurationManager.AppSettings["Database"],
			SslMode = SslMode.Require,
			TrustServerCertificate = true,
		}.ToString();

	}
}
