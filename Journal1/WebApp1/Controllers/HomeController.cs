using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OneTimeUses2;


namespace WebApp1.Controllers
{
	public class HomeController : Controller
	{
		string connectionString = @"mongodb://localhost:27017";

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase("Papers");
			var collection = database.GetCollection<PaperFormatted>("bar1");
			//var filter = Builders<PaperFormatted>.Filter.Eq("i", 71);
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}