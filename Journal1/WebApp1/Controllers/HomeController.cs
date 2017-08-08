using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OneTimeUses2;
using PagedList;
using WebApp1.Models;


namespace WebApp1.Controllers
{
	public class HomeController : Controller
	{
		private string connectionString = @"mongodb://localhost:27017";
		private MongoClient client;
		private IMongoDatabase database;
		private IMongoCollection<PaperViewModel> collection; 
		private DateTime Now = DateTime.MaxValue; //in case the date on the computer is wrong
		public const int MaximumPageSize = 100;
		private const int DefaultDayOfMonth = 1;
		private readonly SortDefinition<PaperViewModel> _sortByDateDesc = Builders<PaperViewModel>.Sort.Descending(p => p.PublicationDate);
		private readonly SortDefinition<PaperViewModel> _sortByTitleDesc = Builders<PaperViewModel>.Sort.Descending(p => p.Title);
		private readonly SortDefinition<PaperViewModel> _sortByJournalNameDesc = Builders<PaperViewModel>.Sort.Descending(p => p.JournalName);


		public HomeController()
		{
			PaperViewModel asd = new PaperViewModel();
			client = new MongoClient(connectionString);
			database = client.GetDatabase("Papers");
			collection = database.GetCollection<PaperViewModel>("bar2");
			
			
		}

		public ActionResult Index()
		{
			return View();
		}
		private List<PaperViewModel> ListByTitle(string s)
		{
			var filter = Builders<PaperViewModel>.Filter.Where(p => p.Title.ToLower().Contains(s.ToLower()));
			var listSorted = collection.Find(filter).Sort(_sortByTitleDesc).ToList();
			var list = collection.Find(filter).ToList();
			return list;
			//return Queryable.Where(collection.AsQueryable(), p => p.Title.ToLower().Contains(s.ToLower())).ToList();
		}
		private List<PaperViewModel> ListByJournal(string s)
		{
			var filter = Builders<PaperViewModel>.Filter.Where(p => p.JournalName.ToLower().Contains(s.ToLower()));
			var list = collection.Find(filter).Sort(_sortByJournalNameDesc).ToList();
			//return list;
			return Queryable.Where(collection.AsQueryable(), p => p.JournalName.ToLower().Contains(s.ToLower())).ToList();
		}
		private List<PaperViewModel> ListByAbstract(string s)
		{
			var filter = Builders<PaperViewModel>.Filter.Where(p => p.Abstract.ToLower().Contains(s.ToLower()));
			var list = collection.Find(filter).Sort(_sortByDateDesc).ToList();
			return list;
			//return Queryable.Where(collection.AsQueryable(), p => p.Abstract.ToLower().Contains(s.ToLower())).ToList();
		}

		private List<PaperViewModel> ListByAuthor(string s)
		{
			var filter = Builders<PaperViewModel>.Filter.Where(p => p.Authors.Any(a => a.ToLower().Contains(s.ToLower())));
			var list = collection.Find(filter).Sort(_sortByDateDesc).ToList();
			return list;
			//return Queryable.Where(collection.AsQueryable(), p => p.Authors.Any(a => a.ToLower().Contains(s.ToLower()))).ToList();
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

		private IEnumerable<PaperViewModel> ListByDateRange(DateTime beginDate, DateTime endDate)
		{

			var filterBuilder = Builders<PaperViewModel>.Filter;

			var filter = filterBuilder.Gte(p => p.PublicationDate, beginDate) & filterBuilder.Lt(p => p.PublicationDate, endDate);
			var pf = collection.Find(filter).ToList();
			return pf;

			//var pf2 = collection.AsQueryable().Where(p => p.PublicationDate.Equals(dt)).ToList();

		}

		private IEnumerable<PaperViewModel> ListByDateDesc()
		{
			
			var filter = Builders<PaperViewModel>.Filter.Lte(p => p.PublicationDate, Now);
			var list = collection.Find(filter).Sort(_sortByDateDesc).ToEnumerable();
			return list;

		}


		//I assume projection makes queries faster but for convience of display will leave off for now. 
		//However, see https://stackoverflow.com/questions/24885811/projection-makes-query-slower to make sure we are not making the same mistakes
		public async Task<ActionResult> About(int page =1 , int pageSize =25, SearchViewModel vm = null)
		{
		//	if (page < 1)
		//	{
		//		page = 1;
		//	}
		//	if (pageSize < 1)
		//	{
		//		pageSize = 1;
		//	}

			vm = vm ?? new SearchViewModel();
			ViewBag.SearchViewModel = vm;

			IEnumerable<PaperViewModel> list = new List<PaperViewModel>();
			if (vm.SearchType == SearchType.Text)
			{
				list = SearchDatabase(vm.SearchField, vm.SearchTerm);
			}
			else if (vm.SearchType == SearchType.Date)
			{
				int offByOneCorrection = 1;
				DateTime beginDate = new DateTime(vm.BeginYear, vm.BeginMonth + offByOneCorrection, DefaultDayOfMonth);
				DateTime endDate = new DateTime(vm.EndYear, vm.EndMonth + offByOneCorrection, DefaultDayOfMonth);
				list = ListByDateRange(beginDate, endDate);
			}
			else
			{
				list = ListByDateDesc();
			}

			var html = GeneratePagingHtml(vm.PageNumber, vm.PageSize, list);

			return View(new PageViewModel {Html = html});
		}

		private string GeneratePagingHtml(int page, int pageSize, IEnumerable<PaperViewModel> list)
		{
			if (pageSize > MaximumPageSize)
			{
				pageSize = MaximumPageSize;
			}

			if (pageSize < 1)
			{
				pageSize = 25;
			}

			if (page < 1)
			{
				page = 1;
			}

			ViewBag.PagingUrlAction = this.ControllerContext.RouteData.Values["action"].ToString();
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;

			ViewBag.Message = "Your application description page.";

			var html = RenderPartialViewToString("Papers", list.ToPagedList(pageNumber: page, pageSize:pageSize));

			return html;
		}


		IList<PaperFormatted> GetPage(List<PaperFormatted> list, int page, int pageSize = 25)
		{
			return list.Skip(page * pageSize).Take(pageSize).ToList();
		}

		private IEnumerable<PaperViewModel> SearchDatabase(string searchField, string searchTerm)
		{
			
		    if(searchField == "Title")
			{
				return ListByTitle(searchTerm);
			}
			else if (searchField == "JournalName")
			{
				return ListByJournal(searchTerm);
			}
			else if (searchField == "Author")
			{
				return ListByAuthor(searchTerm);
			}
			else if (searchField == "Abstract")
			{
				return ListByAbstract(searchTerm);
			}
			else if (String.IsNullOrWhiteSpace(searchTerm) || String.IsNullOrWhiteSpace(searchField))
			{
				return ListByDateDesc();
			}
			
			return  ListByDateDesc();
		}

		public JsonResult Search(SearchViewModel vm)
		{
			vm = vm ?? new SearchViewModel();
			var results = SearchDatabase(vm.SearchField, vm.SearchTerm);

			ViewBag.Query = vm;
			var html = GeneratePagingHtml(vm.PageNumber,vm.PageSize,results);

			return Json(new
			{
				vm,
				html = html
			}, JsonRequestBehavior.AllowGet);
		}


		public JsonResult DateSearch(SearchViewModel vm)
		{
			int offByOneCorrection = 1;
			DateTime beginDate = new DateTime(vm.BeginYear, vm.BeginMonth + offByOneCorrection, DefaultDayOfMonth);
			DateTime endDate = new DateTime(vm.EndYear, vm.EndMonth + offByOneCorrection, DefaultDayOfMonth);
			
			var results = ListByDateRange(beginDate, endDate);

			var html = GeneratePagingHtml(vm.PageNumber, vm.PageSize, results);
			//foreach (var paper in results)
			//{
			//	html += RenderPartialViewToString("Papers", paper);
			//}



			return Json(new
			{
				//beginMonth,
				//beginYear,
				//endMonth,
				//endYear,
				html = html
			}, JsonRequestBehavior.AllowGet);
		}
		protected string RenderPartialViewToString(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = ControllerContext.RouteData.GetRequiredString("action");

			ViewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}


		private IEnumerable<PaperViewModel> SearchDatabase2(string[] searchFields, string[] searchTerms, DateTime begin, DateTime end)
		{
			//what if they are out of order? chuck it into the SearchViewModel?
			var list = collection.AsQueryable();
			int i = 0;
			if (searchFields[i] == "Title")
			{
				list = list.Where(p => p.Title.ToLower().Contains(searchTerms[i].ToLower()));
				i++;
			}
			if (searchFields[i] == "Authors")
			{
				list = list.Where(p => p.Authors.Any(a => a.ToLower().Contains(searchTerms[i].ToLower())));
				i++;
			}
			if (searchFields[i] == "Journal Name")
			{
				list = list.Where(p => p.JournalName.ToLower().Contains(searchTerms[i].ToLower()));
				i++;
			}
			if (searchFields[i] == "Abstract")
			{
				list = list.Where(p => p.Abstract.ToLower().Contains(searchTerms[i].ToLower()));
				i++;
			}
			if (searchFields[i] == "Date")
			{
				list = list.Where(p => p.PublicationDate >= begin && p.PublicationDate <= end);
				i++;
			}
			return list;
		}
	}

	public class SearchViewModel
	{
		public string SearchField { get; set; }
		public string SearchTerm { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		public int BeginMonth { get; set; }
		public int BeginYear { get; set; }

		public int EndMonth { get; set; }
		public int EndYear { get; set; }

		public SearchType SearchType { get; set; }
	}

	public class PageViewModel
	{
		public string Html { get; set; }
	}

	public enum SearchType
	{
		Text = 1,
		Date = 2
	}


}