using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace WebApp1.Models
{
	public class PaperViewModel
	{
		public ObjectId Id { get; set; }
		public string Fields { get; set; } 
		public string PsychArea { get; set; }
		public string KeyTopic { get; set; }
		public string TargetPopulation { get; set; }
		public int Number { get; set; }
		public string Title { get; set; }
		public string[] Authors { get; set; }
		public string JournalName { get; set; }
		public string AlertMonth { get; set; }
		public DateTime PublicationDate { get; set; }
		public string Link { get; set; }
		public string Abstract { get; set; }
	}
}