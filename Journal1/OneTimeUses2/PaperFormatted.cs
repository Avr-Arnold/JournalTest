using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
		public int Number { get; set; } //not necessary but useful for correlating data between VS and Excel
		public string Title { get; set; }
		public List<string> Authors { get; set; }
		public string JournalName { get; set; }
		public string AlertMonth { get; set; }
		public  DateTime PublicationDate { get; set; }
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
			Authors = pi.Authors.Split(',').ToList();
			JournalName = pi.JournalName;
			AlertMonth = pi.AlertMonth;
            SetPublicationDate(pi.PublicationDate);
            Link = pi.Link;
			Abstract = pi.Abstract;
		}

		private void SetPublicationDate(string pubDateString)
		{
			int month = 0;
			int year = 0;
			if (pubDateString.Contains('/'))
			{
				string[] temp = pubDateString.Split('/');
				month = Convert.ToInt32(temp[1]);
				year = Convert.ToInt32(temp[0]) + 2000; //Bec the input years are i.e. 15, 16
				PublicationDate = new DateTime(year, month, 1);
				
			
			}
			else
			{
				PublicationDate = DateTime.MinValue;
			}

		}
	}
}