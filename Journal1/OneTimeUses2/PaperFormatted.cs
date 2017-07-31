using System.Collections.Generic;
using MongoDB.Bson;

namespace OneTimeUses2
{
	public class PaperFormatted //public for web app
	{

		public ObjectId Id { get; set; }
		public string Fields { get; set; } //I would prefer Enums for most, but get it working first
		public string PsychArea { get; set; }
		public string KeyTopic { get; set; }
		public string TargetPopulation { get; set; }
		public int Number { get; set; }
		public string Title { get; set; }
		public string[] Authors { get; set; }
		public string JournalName { get; set; }
		public string AlertMonth { get; set; }
		public string PublicationDate { get; set; }
		public string Link { get; set; }
		public string Abstract { get; set; }


		public PaperFormatted()
		{
		}

		public PaperFormatted(PaperIntake pi)
		{
			
			Id = pi.Id;
			Fields = pi.Fields;
			PsychArea = pi.PsychArea;
			KeyTopic = pi.KeyTopic;
			TargetPopulation = pi.TargetPopulation;
			Number = pi.Number;
			Title = pi.Title;
			Authors = pi.Authors.Split(',');
			JournalName = pi.JournalName;
			AlertMonth = pi.AlertMonth;
			PublicationDate = pi.PublicationDate;
			Link = pi.Link;
			Abstract = pi.Abstract;
		}

		
	}
}