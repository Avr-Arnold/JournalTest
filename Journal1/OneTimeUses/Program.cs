﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CsvHelper;
using System.IO;
using System.Linq;

namespace OneTimeUses
{
    class Program
    {
        static void Main(string[] args)
        {
	        string path = @"C:\Users\Avi\Downloads\mtpp csv.csv";

			Console.WriteLine("Starting main");
	        TextReader textReader = File.OpenText(path);

			var csv = new CsvReader(textReader);
            var records = csv.GetRecords<Paper>().ToList();
            Console.WriteLine("Records main: " + records + " size: " + records.Count);

            foreach (var record in records)
            { 
                //var record = csv.GetRecord<Paper>();
                Console.WriteLine("Records: " + record);
            }

            Console.ReadLine();


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