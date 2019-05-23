<Query Kind="Program">
  <Connection>
    <ID>5b7c562a-4e60-4476-bd27-103a51a73e14</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	var results = BLL_Query();
	// to display the contents of a variable in LinqPad
	// you will use the LinqPad method .Dump()
	results.Dump();
}

// Define other methods and classes here

public IEnumerable<AlbumArtists> BLL_Query()
{
	var results = from a in Albums
					orderby a.Title
					select new AlbumArtists
					{
						Title = a.Title,
						Year = a.ReleaseYear,
						ArtistName = a.Artist.Name
					};
	return results;
}

/*
the query using this class is pulling data from
    multiple tables and is a subset of those tables
the resulting dataset is NOT an entity

the query contains ONLY primitive data values
the query has no data structures (ie list, arrays,...)
classes that are NOT entities and have NO structure
     will be referred to as POCO classes
	 
*/
public class AlbumArtists
{
	public string Title {get; set;}
	public int Year {get;set;}
	public string ArtistName{get;set;}
}














