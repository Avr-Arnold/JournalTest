using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OneTimeUses2;


namespace WebApp1.Controllers
{
	public class HomeController : Controller
	{
		static string connectionString = @"mongodb://localhost:27017";
		static MongoClient client = new MongoClient(connectionString);
		static IMongoDatabase database = client.GetDatabase("Papers");
		IMongoCollection<PaperFormatted> collection = database.GetCollection<PaperFormatted>("bar2");
		private static DateTime Now = DateTime.MaxValue; //in case the date on the computer is wrong

		public ActionResult Index()
		{
			return View();
		}

		private List<PaperFormatted> ListByTopic(string s)
		{
			return collection.AsQueryable().Where(p => p.Abstract.Contains(s)).ToList();
		}

		private List<PaperFormatted> ListByAuthor(string s)
		{
			return collection.AsQueryable().Where(p => p.Authors.Any(a => a.Contains(s))).ToList();
		}

		private List<PaperFormatted> ListByDate(int year, int month)
		{
			DateTime dt = new DateTime(year, month, 1);
			var filter = Builders<PaperFormatted>.Filter.Eq(p => p.PublicationDate, dt);
			return collection.Find(filter).ToList();


			//Additional 2 ways to do this that work.
			//var filterBuilder = Builders<PaperFormatted>.Filter;

			//var filter = filterBuilder.Gte(p => p.PublicationDate, dtBegin) & filterBuilder.Lt(p => p.PublicationDate, dtEnd);
			//var pf = collection.Find(filter).ToList();
			//var pf2 = collection.AsQueryable().Where(p => p.PublicationDate.Equals(dtBegin)).ToList();

		}

		private List<PaperFormatted> ListByDateDesc()
		{
			
			var filter = Builders<PaperFormatted>.Filter.Lte(p => p.PublicationDate, Now);
			var sortedList = Builders<PaperFormatted>.Sort.Descending(p => p.PublicationDate);
			var list = collection.Find(filter).Sort(sortedList).ToList();
			return list;

		}

		public async Task<ActionResult> About()
		{
			//var client = new MongoClient(connectionString);
			//var database = client.GetDatabase("Papers");
			//var collection = database.GetCollection<PaperFormatted>("bar2");

			DateTime dtBegin = new DateTime(2016, 7, 1);
			DateTime dtEnd = new DateTime(2016, 8, 1);
			var list2 = collection.AsQueryable().Where(p => p.Authors.Any(s => s.Contains("Robert"))).ToList();

			var list = collection.AsQueryable().Where(p => p.Abstract.Contains("food")).ToList() ;
			var filterBuilder = Builders<PaperFormatted>.Filter;

			var filter = filterBuilder.Gte(p => p.PublicationDate, dtBegin) & filterBuilder.Lt(p => p.PublicationDate, dtEnd);
			var pf =  collection.Find(filter).ToList();
			var pf2 = collection.AsQueryable().Where(p => p.PublicationDate.Equals(dtBegin)).ToList();
			var dateTest = ListByDateDesc();
			ViewBag.Message = "Your application description page.";
			//ViewBag.Documents = list;
			//ViewBag.Id = pf.Id;
			//ViewBag.Fields = pf.Fields;
			//ViewBag.PsychArea = pf.PsychArea;
			//ViewBag.KeyTopic = pf.KeyTopic;
			//ViewBag.TargetPopulation = pf.TargetPopulation;
			//ViewBag.Title = pf.Title;
			//ViewBag.Authors = pf.Authors;
			//ViewBag.JournalName = pf.JournalName;
			//ViewBag.AlertMonth = pf.AlertMonth;
			//ViewBag.PublicationDate = pf.PublicationDate;
			//ViewBag.Link = pf.Link;
			//ViewBag.Abstract = pf.Abstract;

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}