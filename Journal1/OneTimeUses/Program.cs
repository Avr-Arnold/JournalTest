using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace OneTimeUses
{
    class Program
    {
        static void Main(string[] args)
        {
			BsonClassMap.RegisterClassMap<Paper>();
			var client = new MongoClient("mongodb://localhost:27017");
			//using (var cursor = client.ListDatabases())
			//{
			//	foreach (var document in cursor.ToEnumerable())
			//	{
			//		Console.WriteLine(document.ToString());
			//	}
			//}
			var database = client.GetDatabase("Papers");
			var collection = database.GetCollection<Paper>("bar");
			

			
			Console.WriteLine("Hello World!");
	        Console.Read();
        }
    }

}