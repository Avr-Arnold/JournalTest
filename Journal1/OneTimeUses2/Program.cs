using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CsvHelper;
using System.IO;
using System.Linq;


namespace OneTimeUses2
{
    class Program
    {
        static void Main(string[] args)
        {
	        string path = @"C:\Users\Avi\Downloads\mtpp2.csv";
	        string connectionString = @"mongodb://localhost:27017";

			Console.WriteLine("Starting main");
	        TextReader textReader = File.OpenText(path);

			var csv = new CsvReader(textReader);
            var records = csv.GetRecords<PaperIntake>().ToList();
            Console.WriteLine("Records main: " + records + " size: " + records.Count);
			PaperFormatted[] pfArray = new PaperFormatted[records.Count];
	        int i = 0;
            foreach(PaperIntake record in records)
            { 
                pfArray[i++] = new PaperFormatted(record);
            }

            Console.ReadLine();


            BsonClassMap.RegisterClassMap<PaperFormatted>();
			var client = new MongoClient(connectionString);
			//using (var cursor = client.ListDatabases())
			//{
			//	foreach (var document in cursor.ToEnumerable())
			//	{
			//		Console.WriteLine(document.ToString());
			//	}
			//}
			var database = client.GetDatabase("Papers");
			var collection = database.GetCollection<PaperFormatted>("bar1");
	        
	        foreach (PaperFormatted pf in pfArray)
			{

				collection.InsertOne(pf);
	        }

			
			Console.WriteLine("Hello World!");
	        Console.Read();
        }
    }

}