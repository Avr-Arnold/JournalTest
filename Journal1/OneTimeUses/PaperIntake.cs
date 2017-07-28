using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace OneTimeUses
{
    class PaperIntake
    {
	    public ObjectId Id { get; set; }
	    public string Fields { get; set; } //I would prefer Enums for most, but get it working first
	    public string PsychArea { get; set; }
	    public string KeyTopic { get; set; }
	    public string TargetPopulation { get; set; }
	    public int Number { get; set; }
	    public string Title { get; set; }
	    public string Authors { get; set; }
	    public string JournalName { get; set; }
	    public string AlertMonth { get; set; }
	    public string PublicationDate { get; set; }
	    public string Link { get; set; }
	    public string Abstract { get; set; }


	}
}
