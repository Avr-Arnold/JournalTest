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
		public async Task<ActionResult> About()
		{
			//var client = new MongoClient(connectionString);
			//var database = client.GetDatabase("Papers");
			//var collection = database.GetCollection<PaperFormatted>("bar2");

			DateTime dt = new DateTime(2016, 7, 1);
			var list2 = collection.AsQueryable().Where(p => p.Authors.Any(s => s.Contains("Robert"))).ToList();

			var list = collection.AsQueryable().Where(p => p.Abstract.Contains("food")).ToList() ;
			var filter = Builders<PaperFormatted>.Filter.Eq(p => p.PublicationDate, dt);
			var pf =  collection.Find(filter).Limit(10);

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