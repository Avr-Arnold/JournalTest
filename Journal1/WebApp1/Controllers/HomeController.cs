using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OneTimeUses2;
using PagedList;
using WebApp1.Models;


namespace WebApp1.Controllers
{
	public class HomeController : Controller
	{
		static string connectionString = @"mongodb://localhost:27017";
		static MongoClient client = new MongoClient(connectionString);
		static IMongoDatabase database = client.GetDatabase("Papers");
		IMongoCollection<PaperViewModel> collection = database.GetCollection<PaperViewModel>("bar2");
		private static DateTime Now = DateTime.MaxValue; //in case the date on the computer is wrong
		public const int MaximumPageSize = 100;

		public ActionResult Index()
		{
			return View();
		}
		private List<PaperViewModel> ListByTitle(string s)
		{
			return collection.AsQueryable().Where(p => p.Title.Contains(s)).ToList();
		}
		private List<PaperViewModel> ListByJournal(string s)
		{
			return collection.AsQueryable().Where(p => p.JournalName.Contains(s)).ToList();
		}
		private List<PaperViewModel> ListByTopic(string s)
		{
			return collection.AsQueryable().Where(p => p.Abstract.Contains(s)).ToList();
		}

		private List<PaperViewModel> ListByAuthor(string s)
		{
			return collection.AsQueryable().Where(p => p.Authors.Any(a => a.Contains(s))).ToList();
		}

		private IEnumerable<PaperViewModel> ListByDate(int year, int month)
		{
			DateTime dt = new DateTime(year, month, 1);
			var filter = Builders<PaperViewModel>.Filter.Eq(p => p.PublicationDate, dt);
			return collection.Find(filter).ToEnumerable();


			//Additional 2 ways to do this that work.
			//DateTime dtBegin = new DateTime(2016, 7, 1);
			//DateTime dtEnd = new DateTime(2016, 8, 1);
			//var filterBuilder = Builders<PaperFormatted>.Filter;

			//var filter = filterBuilder.Gte(p => p.PublicationDate, dtBegin) & filterBuilder.Lt(p => p.PublicationDate, dtEnd);
			//var pf = collection.Find(filter).ToList();
			//var pf2 = collection.AsQueryable().Where(p => p.PublicationDate.Equals(dt)).ToList();

		}

		private IEnumerable<PaperViewModel> ListByDateDesc()
		{
			
			var filter = Builders<PaperViewModel>.Filter.Lte(p => p.PublicationDate, Now);
			var sortedList = Builders<PaperViewModel>.Sort.Descending(p => p.PublicationDate);
			var list = collection.Find(filter).Sort(sortedList).ToEnumerable();
			return list;

		}

		private IEnumerable<PaperViewModel> Search(List<FilterViewModel> filters)
		{

			var filter = Builders<PaperViewModel>.Filter.Lte(p => p.PublicationDate, Now);
			var sortedList = Builders<PaperViewModel>.Sort.Descending(p => p.PublicationDate);
			var list = collection.Find(filter).Sort(sortedList).ToEnumerable();
			return list;

		}

		//I assume projection makes queries faster but for convience of display will leave off for now. 
		//However, see https://stackoverflow.com/questions/24885811/projection-makes-query-slower to make sure we are not making the same mistakes
		public async Task<ActionResult> About(int page = 1 , int pageSize = 25)
		{
			
			
			var dateTest = ListByDateDesc();
			if (pageSize > MaximumPageSize)
			{
				pageSize = MaximumPageSize;
			}
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;

			ViewBag.Message = "Your application description page.";
			
            return View(dateTest.ToPagedList(page,pageSize));
		}

		IList<PaperFormatted> GetPage(List<PaperFormatted> list, int page, int pageSize = 25)
		{
			return list.Skip(page * pageSize).Take(pageSize).ToList();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}

	public class FilterViewModel
	{
		public string Property { get; set; }
	}
}