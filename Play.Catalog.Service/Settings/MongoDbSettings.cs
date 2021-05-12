using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Settings
{
	public class MongoDbSettings
	{
		// init prevents from modification afterwards 
		public string Host
		{
			get;
			init;
		}

		public int Port
		{
			get;
			init;
		}

		public string ConnectionString => $"mongodb://{Host}:{Port}";
	}
}
