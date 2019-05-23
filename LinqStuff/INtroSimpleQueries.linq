<Query Kind="Expression">
  <Connection>
    <ID>5b7c562a-4e60-4476-bd27-103a51a73e14</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Chinook</Database>
  </Connection>
</Query>

/* using query syntax, list all the records from the Albums table

from x in Albums
select x
*/

/* using method syntax, list all the records from the Albums table
Albums.Select (x => x)
*/

/* using query syntax, list all the records from the Albums table for ArtistId 1

from x in Albums
where x.ArtistId == 1
select x
*/

/* using method syntax, list all the records from the Albums table for ArtistId 1

Albums.Where (x => (x.ArtistId == 1))
*/

/* using query syntax, list all the records from the Albums table for ArtistId 1 
   ordered ascending by ReleaseYear

from x in Albums
where x.ArtistId == 1
orderby x.ReleaseYear
select x
*/

/* using method syntax, list all the records from the Albums table for ArtistId 1
	ordered ascending by ReleaseYear

Albums.Where (x => (x.ArtistId == 1)).OrderBy(x => x.ReleaseYear)
*/

/* list all records fromt he entity Albums ordered by descending ReleaseYear
   and alphbetically by Title 
   descending is required
   ascending is the default for any sort item/column 

from x in Albums
orderby x.ReleaseYear descending, x.Title ascending
select x

Albums
   .OrderByDescending (x => x.ReleaseYear)
   .ThenBy (x => x.Title)*/

/* repeat the previous query but only for years 2007 thru 2010 inclusive
from x in Albums
where x.ReleaseYear >= 2007 && x.ReleaseYear <= 2010
orderby x.ReleaseYear descending, x.Title ascending

select x

Albums
   .OrderByDescending (x => x.ReleaseYear)
   .ThenBy (x => x.Title)
	.Where(x => (x.ReleaseYear >= 2007 && x.ReleaseYear <= 2010))
*/

/*
 list all customers in alphabetic order by last name, firstname who live in the US
  with an yahoo email

Customers
	.Where(c => (c.Country.Equals("USA") && c.Email.Contains("yahoo")))
	.OrderBy(c => (c.LastName))
	.ThenBy(c => (c.FirstName))
	
from c in Customers
where c.Country.Equals("USA") && c.Email.Contains("yahoo")
orderby c.LastName, c.FirstName
select c*/

/* one can query for a subset of entity attributes
   one can query for a set of attributes from multiple entities
   
   create a query to return ONLY the trackid and song name for use
   by a dropdownlist
   
from t in Tracks
orderby t.Name
select  new
{
	TrackId = t.TrackId,
	Song = t.Name
}*/

/*
	Create an alphabetic list of albums showing the album title
	releasyear and artist name
*/

from a in Albums
orderby a.Title
select new
{
	Title = a.Title,
	Year = a.ReleaseYear,
	ArtistName = a.Artist.Name
}

























